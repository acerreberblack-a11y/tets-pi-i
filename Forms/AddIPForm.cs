using System;
using System.Linq;
using System.Windows.Forms;
using IPWhiteListManager.Data;
using IPWhiteListManager.Models;

namespace IPWhiteListManager.Forms
{
    public partial class AddIPForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly bool _isEditMode;
        private readonly IPAddressInfo _ipToEdit;
        private readonly SystemInfo _systemForEdit;

        private bool _isUpdatingCombinedState;

        public AddIPForm(DatabaseManager dbManager)
            : this(dbManager, null, null)
        {
        }

        public AddIPForm(DatabaseManager dbManager, IPAddressInfo ipToEdit, SystemInfo systemForEdit)
        {
            _dbManager = dbManager;
            _ipToEdit = ipToEdit;
            _systemForEdit = systemForEdit;
            _isEditMode = ipToEdit != null;

            InitializeComponent();

            // Подписка на события
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            txtIPAddress.TextChanged += TxtIPAddress_TextChanged;
            txtRequestNumber.TextChanged += TxtRequestNumber_TextChanged;
            chkCombined.CheckedChanged += ChkCombined_CheckedChanged;
            chkProduction.CheckedChanged += EnvironmentCheckBoxChanged;
            chkTest.CheckedChanged += EnvironmentCheckBoxChanged;

            LoadSystems();

            if (_isEditMode)
            {
                Text = "Редактирование IP";
                btnSave.Text = "Сохранить";
                PopulateFromExisting();
            }
        }

        private void LoadSystems()
        {
            var systems = _dbManager.GetAllSystems();
            cmbSystem.Items.Clear();

            foreach (var system in systems)
            {
                cmbSystem.Items.Add(system.SystemName);
            }
        }

        private void TxtIPAddress_TextChanged(object sender, EventArgs e)
        {
            CheckExistingIP();
        }

        private void TxtRequestNumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRequestNumber.Text.Trim()))
            {
                chkRegisterNamen.Checked = false;
                chkRegisterNamen.Enabled = false;
            }
            else
            {
                chkRegisterNamen.Enabled = !_isEditMode || (_ipToEdit != null && !_ipToEdit.IsRegisteredInNamen);
            }
        }

        private void CheckExistingIP()
        {
            string ipAddress = txtIPAddress.Text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
                return;

            var existingIPs = _dbManager
                .FindIPAddresses(ipAddress)
                .Where(ip => !_isEditMode || ip.Id != _ipToEdit.Id)
                .ToList();

            if (existingIPs.Any())
            {
                var firstIP = existingIPs.First();

                cmbSystem.Text = firstIP.SystemName;
                cmbSystem.Enabled = false;

                switch (firstIP.Environment)
                {
                    case EnvironmentType.Production:
                        chkProduction.Checked = true;
                        chkProduction.Enabled = false;
                        chkTest.Checked = false;
                        chkTest.Enabled = true;
                        SetCombinedState(false, false);
                        chkCombined.Enabled = false;
                        break;
                    case EnvironmentType.Test:
                        chkProduction.Checked = false;
                        chkProduction.Enabled = true;
                        chkTest.Checked = true;
                        chkTest.Enabled = false;
                        SetCombinedState(false, false);
                        chkCombined.Enabled = false;
                        break;
                    case EnvironmentType.Both:
                        chkProduction.Checked = true;
                        chkProduction.Enabled = false;
                        chkTest.Checked = true;
                        chkTest.Enabled = false;
                        SetCombinedState(true, false);
                        chkCombined.Enabled = false;
                        break;
                }

                MessageBox.Show($"IP-адрес {ipAddress} уже существует в системе {firstIP.SystemName} ({firstIP.Environment}).\n" +
                               "Система и текущее окружение заблокированы для редактирования.",
                               "IP-адрес найден",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                cmbSystem.Enabled = true;
                chkProduction.Enabled = true;
                chkTest.Enabled = true;
                chkCombined.Enabled = true;
                SetCombinedState(false, false);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbSystem.Text))
            {
                MessageBox.Show("Введите или выберите систему", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtIPAddress.Text))
            {
                MessageBox.Show("Введите IP-адрес", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!chkProduction.Checked && !chkTest.Checked)
            {
                MessageBox.Show("Выберите хотя бы одно окружение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var environment = GetEnvironmentType();
            var systemId = GetOrCreateSystem(cmbSystem.Text.Trim());
            if (systemId == -1)
            {
                MessageBox.Show("Не удалось создать или выбрать ИС. Повторите попытку после добавления системы.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ipInfo = new IPAddressInfo
            {
                Id = _ipToEdit?.Id ?? 0,
                SystemId = systemId,
                IPAddress = txtIPAddress.Text.Trim(),
                Environment = environment,
                IsRegisteredInNamen = CalculateRegistrationState(),
                NamenRequestNumber = string.IsNullOrEmpty(txtRequestNumber.Text.Trim()) ? null : txtRequestNumber.Text.Trim()
            };

            try
            {
                if (_isEditMode)
                {
                    _dbManager.UpdateIPAddress(ipInfo);

                    if (ShouldRegisterInNamen())
                    {
                        RegisterInNamen(ipInfo.Id, ipInfo.IPAddress, environment.ToString());
                    }
                    else
                    {
                        MessageBox.Show("IP-адрес успешно обновлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    var ipId = _dbManager.AddIPAddress(ipInfo);

                    if (chkRegisterNamen.Checked && string.IsNullOrEmpty(txtRequestNumber.Text.Trim()))
                    {
                        RegisterInNamen(ipId, ipInfo.IPAddress, environment.ToString());
                    }
                    else
                    {
                        MessageBox.Show("IP-адрес успешно добавлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении IP-адреса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetOrCreateSystem(string systemName)
        {
            var systems = _dbManager.GetAllSystems();
            var existingSystem = systems.FirstOrDefault(s => s.SystemName.Equals(systemName, StringComparison.OrdinalIgnoreCase));

            if (existingSystem != null)
            {
                return existingSystem.Id;
            }

            using (var addSystemForm = new AddSystemForm(_dbManager, systemName))
            {
                if (addSystemForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSystems();

                    var createdSystem = addSystemForm.CreatedSystem ?? _dbManager.FindSystemByName(systemName);
                    if (createdSystem != null)
                    {
                        cmbSystem.SelectedItem = createdSystem.SystemName;
                        return createdSystem.Id;
                    }
                }
            }

            return -1;
        }

        private void ChkCombined_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingCombinedState)
            {
                return;
            }

            _isUpdatingCombinedState = true;
            try
            {
                if (chkCombined.Checked)
                {
                    chkProduction.Checked = true;
                    chkTest.Checked = true;
                    chkProduction.Enabled = false;
                    chkTest.Enabled = false;
                }
                else
                {
                    chkProduction.Enabled = true;
                    chkTest.Enabled = true;
                }
            }
            finally
            {
                _isUpdatingCombinedState = false;
            }
        }

        private void EnvironmentCheckBoxChanged(object sender, EventArgs e)
        {
            if (_isUpdatingCombinedState)
            {
                return;
            }

            var shouldCombine = chkProduction.Checked && chkTest.Checked;
            SetCombinedState(shouldCombine, false);
        }

        private void SetCombinedState(bool isCombined, bool adjustEnabled)
        {
            _isUpdatingCombinedState = true;
            try
            {
                if (chkCombined.Checked != isCombined)
                {
                    chkCombined.Checked = isCombined;
                }

                if (adjustEnabled)
                {
                    chkProduction.Enabled = !isCombined;
                    chkTest.Enabled = !isCombined;
                }
            }
            finally
            {
                _isUpdatingCombinedState = false;
            }
        }

        private EnvironmentType GetEnvironmentType()
        {
            if (chkProduction.Checked && chkTest.Checked)
                return EnvironmentType.Both;
            else if (chkProduction.Checked)
                return EnvironmentType.Production;
            else
                return EnvironmentType.Test;
        }

        private async void RegisterInNamen(int ipAddressId, string ipAddress, string environment)
        {
            try
            {
                var namenService = new Services.NamenService();
                var result = await namenService.RegisterIPInNamen(ipAddress, environment);

                if (result.Success)
                {
                    _dbManager.UpdateNamenRegistration(ipAddressId, true, result.RequestNumber);
                    MessageBox.Show($"IP {ipAddress} успешно зарегистрирован в namen\nНомер заявки: {result.RequestNumber}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Не удалось зарегистрировать IP {ipAddress} в namen", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации в namen: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PopulateFromExisting()
        {
            if (_systemForEdit != null)
            {
                cmbSystem.SelectedItem = _systemForEdit.SystemName;
            }
            else
            {
                cmbSystem.SelectedItem = _ipToEdit.SystemName;
                if (cmbSystem.SelectedItem == null)
                {
                    cmbSystem.Text = _ipToEdit.SystemName;
                }
            }

            txtIPAddress.Text = _ipToEdit.IPAddress;
            txtRequestNumber.Text = _ipToEdit.NamenRequestNumber;

            chkRegisterNamen.Checked = false;
            chkRegisterNamen.Enabled = !_ipToEdit.IsRegisteredInNamen && string.IsNullOrEmpty(_ipToEdit.NamenRequestNumber);

            PopulateEnvironmentFromEdit();
        }

        private void PopulateEnvironmentFromEdit()
        {
            if (!_isEditMode || _ipToEdit == null)
            {
                return;
            }

            switch (_ipToEdit.Environment)
            {
                case EnvironmentType.Production:
                    chkProduction.Checked = true;
                    chkTest.Checked = false;
                    SetCombinedState(false, true);
                    break;
                case EnvironmentType.Test:
                    chkProduction.Checked = false;
                    chkTest.Checked = true;
                    SetCombinedState(false, true);
                    break;
                case EnvironmentType.Both:
                    chkProduction.Checked = true;
                    chkTest.Checked = true;
                    SetCombinedState(true, true);
                    break;
            }
        }

        private bool CalculateRegistrationState()
        {
            if (!string.IsNullOrEmpty(txtRequestNumber.Text.Trim()))
            {
                return true;
            }

            if (_isEditMode && _ipToEdit.IsRegisteredInNamen)
            {
                return true;
            }

            return chkRegisterNamen.Checked;
        }

        private bool ShouldRegisterInNamen()
        {
            if (!_isEditMode || _ipToEdit == null)
            {
                return false;
            }

            return chkRegisterNamen.Checked &&
                   string.IsNullOrEmpty(txtRequestNumber.Text.Trim()) &&
                   !_ipToEdit.IsRegisteredInNamen;
        }
    }
}
