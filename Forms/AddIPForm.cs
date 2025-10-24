using System;
using System.Collections.Generic;
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
        private readonly AutoCompleteStringCollection _systemAutoComplete = new AutoCompleteStringCollection();

        private SystemInfo _selectedSystem;

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
            btnEditSystem.Click += BtnEditSystem_Click;
            txtIPAddress.TextChanged += TxtIPAddress_TextChanged;
            txtRequestNumber.TextChanged += TxtRequestNumber_TextChanged;
            chkCombined.CheckedChanged += ChkCombined_CheckedChanged;
            chkProduction.CheckedChanged += EnvironmentCheckBoxChanged;
            chkTest.CheckedChanged += EnvironmentCheckBoxChanged;
            cmbSystem.SelectedIndexChanged += CmbSystem_SelectedIndexChanged;
            cmbSystem.TextChanged += CmbSystem_TextChanged;
            chkRegisterNamen.CheckedChanged += ChkRegisterNamen_CheckedChanged;

            toolTip.SetToolTip(cmbSystem, "Начните вводить название для быстрого поиска или добавьте новую ИС");
            toolTip.SetToolTip(btnEditSystem, "Открыть карточку выбранной ИС или создать новую");
            toolTip.SetToolTip(txtIPAddress, "Укажите один IP-адрес в формате IPv4");
            toolTip.SetToolTip(chkCombined, "Автоматически отметит прод и тест, если IP общий");
            toolTip.SetToolTip(txtRequestNumber, "Если IP уже зарегистрирован в namen, укажите номер заявки");
            toolTip.SetToolTip(chkRegisterNamen, "Отправить заявку в namen сразу после сохранения IP");

            LoadSystems();
            UpdateSystemDetails();
            UpdateNamenStatusLabel();

            if (_isEditMode)
            {
                Text = "Редактирование IP";
                btnSave.Text = "Сохранить";
                PopulateFromExisting();
            }
        }

        private void LoadSystems()
        {
            var currentText = cmbSystem.Text;

            var systems = _dbManager.GetAllSystems();
            cmbSystem.Items.Clear();

            _systemAutoComplete.Clear();

            foreach (var system in systems)
            {
                cmbSystem.Items.Add(system.SystemName);
                _systemAutoComplete.Add(system.SystemName);
            }

            cmbSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbSystem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            cmbSystem.AutoCompleteCustomSource = _systemAutoComplete;

            if (!string.IsNullOrWhiteSpace(currentText))
            {
                var index = cmbSystem.FindStringExact(currentText);
                if (index >= 0)
                {
                    cmbSystem.SelectedIndex = index;
                }
                else
                {
                    cmbSystem.Text = currentText;
                }
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

            UpdateNamenStatusLabel();
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

                UpdateSystemDetails();
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
                UpdateSystemDetails();
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
            UpdateSystemDetails();
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
                _selectedSystem = existingSystem;
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
                        UpdateSystemDetails();
                        _selectedSystem = createdSystem;
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
            lblNamenStatus.Text = _ipToEdit.IsRegisteredInNamen
                ? "Статус регистрации: ✓ зарегистрирован"
                : (!string.IsNullOrEmpty(_ipToEdit.NamenRequestNumber)
                    ? $"Статус регистрации: заявка {_ipToEdit.NamenRequestNumber}"
                    : "Статус регистрации: возможно регистрация");

            PopulateEnvironmentFromEdit();
            UpdateSystemDetails();
            UpdateNamenStatusLabel();
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

        private void UpdateNamenStatusLabel()
        {
            if (_isEditMode && _ipToEdit != null)
            {
                if (_ipToEdit.IsRegisteredInNamen)
                {
                    lblNamenStatus.Text = "Статус регистрации: ✓ зарегистрирован";
                }
                else if (!string.IsNullOrEmpty(_ipToEdit.NamenRequestNumber))
                {
                    lblNamenStatus.Text = $"Статус регистрации: заявка {_ipToEdit.NamenRequestNumber}";
                }
                else if (!string.IsNullOrWhiteSpace(txtRequestNumber.Text))
                {
                    lblNamenStatus.Text = $"Статус регистрации: заявка {txtRequestNumber.Text.Trim()}";
                }
                else if (chkRegisterNamen.Checked)
                {
                    lblNamenStatus.Text = "Статус регистрации: заявка будет создана";
                }
                else
                {
                    lblNamenStatus.Text = "Статус регистрации: без автоматической заявки";
                }

                return;
            }

            if (!string.IsNullOrWhiteSpace(txtRequestNumber.Text))
            {
                lblNamenStatus.Text = $"Статус регистрации: заявка {txtRequestNumber.Text.Trim()}";
            }
            else if (chkRegisterNamen.Checked)
            {
                lblNamenStatus.Text = "Статус регистрации: заявка будет создана";
            }
            else
            {
                lblNamenStatus.Text = "Статус регистрации: ручной контроль";
            }
        }

        private void ChkRegisterNamen_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNamenStatusLabel();
        }

        private void UpdateSystemDetails()
        {
            var systemName = cmbSystem.Text?.Trim();

            if (string.IsNullOrEmpty(systemName))
            {
                txtSystemDetails.Text = "Введите или выберите систему, чтобы увидеть подробности.";
                btnEditSystem.Enabled = false;
                _selectedSystem = null;
                return;
            }

            var system = _dbManager.FindSystemByName(systemName);

            if (system == null)
            {
                txtSystemDetails.Text = "Система не найдена в справочнике. Нажмите \"Инфо об ИС...\", чтобы добавить её.";
                btnEditSystem.Enabled = true;
                _selectedSystem = null;
                return;
            }

            _selectedSystem = system;

            var infoLines = new List<string>();

            infoLines.Add($"Наименование: {system.SystemName}");

            if (!string.IsNullOrWhiteSpace(system.Description))
            {
                infoLines.Add($"Описание: {system.Description}");
            }

            var ownerInfo = FormatContact("Владелец", system.OwnerName, system.OwnerEmail);
            if (!string.IsNullOrEmpty(ownerInfo))
            {
                infoLines.Add(ownerInfo);
            }

            var technicalInfo = FormatContact("Технический специалист", system.TechnicalSpecialistName, system.TechnicalSpecialistEmail);
            if (!string.IsNullOrEmpty(technicalInfo))
            {
                infoLines.Add(technicalInfo);
            }

            var curatorInfo = FormatContact("Куратор", system.CuratorName, system.CuratorEmail);
            if (!string.IsNullOrEmpty(curatorInfo))
            {
                infoLines.Add(curatorInfo);
            }

            infoLines.Add($"Объединенные контуры: {(system.IsTestProductionCombined ? "Да" : "Нет")}");

            txtSystemDetails.Text = string.Join(Environment.NewLine, infoLines);
            btnEditSystem.Enabled = true;
        }

        private static string FormatContact(string role, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email))
            {
                return $"{role}: {name} ({email})";
            }

            return !string.IsNullOrWhiteSpace(name)
                ? $"{role}: {name}"
                : $"{role}: {email}";
        }

        private void CmbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSystemDetails();
        }

        private void CmbSystem_TextChanged(object sender, EventArgs e)
        {
            UpdateSystemDetails();
        }

        private void BtnEditSystem_Click(object sender, EventArgs e)
        {
            var systemName = cmbSystem.Text?.Trim();

            if (string.IsNullOrEmpty(systemName))
            {
                MessageBox.Show("Сначала выберите или введите наименование ИС", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SystemInfo system = _selectedSystem ?? _dbManager.FindSystemByName(systemName);

            if (system == null)
            {
                using (var addSystemForm = new AddSystemForm(_dbManager, systemName))
                {
                    if (addSystemForm.ShowDialog(this) == DialogResult.OK)
                    {
                        ReloadSystemsAndSelect(systemName);
                    }
                }
                return;
            }

            using (var editSystemForm = new AddSystemForm(_dbManager, system))
            {
                if (editSystemForm.ShowDialog(this) == DialogResult.OK)
                {
                    var updatedName = editSystemForm.CreatedSystem?.SystemName ?? system.SystemName;
                    ReloadSystemsAndSelect(updatedName);
                }
            }
        }

        private void ReloadSystemsAndSelect(string systemName)
        {
            LoadSystems();

            if (!string.IsNullOrWhiteSpace(systemName))
            {
                var index = cmbSystem.FindStringExact(systemName);
                if (index >= 0)
                {
                    cmbSystem.SelectedIndex = index;
                }
                else
                {
                    cmbSystem.Text = systemName;
                }
            }

            UpdateSystemDetails();
        }
    }
}
