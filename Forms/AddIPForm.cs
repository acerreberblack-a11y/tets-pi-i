using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private bool _currentIpExistsInProduction;
        private bool _currentIpExistsInTest;
        private IPAddressInfo _existingIpMatch;

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
            btnAddIp.Click += BtnAddIp_Click;
            btnRemoveIp.Click += BtnRemoveIp_Click;
            dgvIpAddresses.SelectionChanged += DgvIpAddresses_SelectionChanged;
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
            toolTip.SetToolTip(btnAddIp, "Добавить IP-адрес в таблицу для пакетного сохранения");
            toolTip.SetToolTip(btnRemoveIp, "Удалить выбранный IP из списка");
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
                ToggleMultiIpControls(false);
                PopulateFromExisting();
            }
            else
            {
                ToggleMultiIpControls(true);
                UpdateIpListControls();
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
            CheckExistingIP(txtIPAddress.Text.Trim(), adjustFormState: true, showExistingMessage: false);
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

        private bool CheckExistingIP(string ipAddress, bool adjustFormState, bool showExistingMessage)
        {
            _existingIpMatch = null;
            _currentIpExistsInProduction = false;
            _currentIpExistsInTest = false;

            if (string.IsNullOrEmpty(ipAddress))
            {
                if (adjustFormState)
                {
                    cmbSystem.Enabled = true;
                    chkProduction.Enabled = true;
                    chkTest.Enabled = true;
                    chkCombined.Enabled = true;
                    SetCombinedState(false, true);
                    UpdateSystemDetails();
                    RefreshPendingIpEnvironmentColumn();
                }

                return true;
            }

            var existingIPs = _dbManager
                .FindIPAddresses(ipAddress)
                .Where(ip => !_isEditMode || ip.Id != _ipToEdit.Id)
                .ToList();

            if (existingIPs.Any())
            {
                var firstIP = existingIPs.First();
                _existingIpMatch = firstIP;

                _currentIpExistsInProduction = existingIPs.Any(ip =>
                    ip.Environment == EnvironmentType.Production || ip.Environment == EnvironmentType.Both);
                _currentIpExistsInTest = existingIPs.Any(ip =>
                    ip.Environment == EnvironmentType.Test || ip.Environment == EnvironmentType.Both);

                var isCombined = _currentIpExistsInProduction && _currentIpExistsInTest;

                if (adjustFormState)
                {
                    cmbSystem.Text = firstIP.SystemName;
                    cmbSystem.Enabled = false;

                    _isUpdatingCombinedState = true;
                    try
                    {
                        chkProduction.Checked = _currentIpExistsInProduction;
                        chkTest.Checked = _currentIpExistsInTest;
                    }
                    finally
                    {
                        _isUpdatingCombinedState = false;
                    }

                    if (isCombined)
                    {
                        SetCombinedState(true, true);
                        chkCombined.Enabled = true;
                    }
                    else
                    {
                        SetCombinedState(false, true);
                        chkCombined.Enabled = false;
                    }

                    UpdateSystemDetails();
                    RefreshPendingIpEnvironmentColumn();
                }

                if (showExistingMessage)
                {
                    MessageBox.Show($"IP-адрес {ipAddress} уже существует в системе {firstIP.SystemName} ({firstIP.Environment})",
                        "IP-адрес найден", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return false;
            }

            if (adjustFormState)
            {
                cmbSystem.Enabled = true;
                chkProduction.Enabled = true;
                chkTest.Enabled = true;
                chkCombined.Enabled = true;
                SetCombinedState(false, true);
                UpdateSystemDetails();
            }

            return true;
        }


        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!EnsurePendingIpCaptured())
            {
                return;
            }

            if (string.IsNullOrEmpty(cmbSystem.Text))
            {
                MessageBox.Show("Введите или выберите систему", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_isEditMode)
            {
                if (string.IsNullOrEmpty(txtIPAddress.Text))
                {
                    MessageBox.Show("Введите IP-адрес", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (GetIpEntries().Count == 0 && _existingIpMatch == null)
            {
                MessageBox.Show("Добавьте хотя бы один IP-адрес в таблицу", "Ошибка",
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

            try
            {
                if (_isEditMode)
                {
                    var ipInfo = new IPAddressInfo
                    {
                        Id = _ipToEdit?.Id ?? 0,
                        SystemId = systemId,
                        IPAddress = txtIPAddress.Text.Trim(),
                        Environment = environment,
                        IsRegisteredInNamen = CalculateRegistrationState(),
                        NamenRequestNumber = string.IsNullOrEmpty(txtRequestNumber.Text.Trim()) ? null : txtRequestNumber.Text.Trim()
                    };

                    _dbManager.UpdateIPAddress(ipInfo);

                    if (ShouldRegisterInNamen())
                    {
                        await RegisterInNamenAsync(new List<IPAddressInfo> { ipInfo }, environment.ToString());
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
                    var pendingIps = GetIpEntries();
                    var createdIps = new List<IPAddressInfo>();
                    IPAddressInfo updatedExisting = null;

                    if (_existingIpMatch != null)
                    {
                        updatedExisting = new IPAddressInfo
                        {
                            Id = _existingIpMatch.Id,
                            SystemId = systemId,
                            IPAddress = _existingIpMatch.IPAddress,
                            Environment = environment,
                            IsRegisteredInNamen = _existingIpMatch.IsRegisteredInNamen,
                            NamenRequestNumber = string.IsNullOrEmpty(txtRequestNumber.Text.Trim())
                                ? _existingIpMatch.NamenRequestNumber
                                : txtRequestNumber.Text.Trim()
                        };

                        _dbManager.UpdateIPAddress(updatedExisting);
                    }

                    foreach (var ipAddress in pendingIps)
                    {
                        var ipInfo = new IPAddressInfo
                        {
                            SystemId = systemId,
                            IPAddress = ipAddress,
                            Environment = environment,
                            IsRegisteredInNamen = CalculateRegistrationState(),
                            NamenRequestNumber = string.IsNullOrEmpty(txtRequestNumber.Text.Trim()) ? null : txtRequestNumber.Text.Trim()
                        };

                        var ipId = _dbManager.AddIPAddress(ipInfo);
                        ipInfo.Id = ipId;
                        createdIps.Add(ipInfo);
                    }

                    var registerRequested = chkRegisterNamen.Checked && string.IsNullOrEmpty(txtRequestNumber.Text.Trim());

                    if (registerRequested)
                    {
                        var ipsToRegister = new List<IPAddressInfo>();

                        if (updatedExisting != null && !_existingIpMatch.IsRegisteredInNamen)
                        {
                            ipsToRegister.Add(updatedExisting);
                        }

                        ipsToRegister.AddRange(createdIps);

                        if (ipsToRegister.Count > 0)
                        {
                            await RegisterInNamenAsync(ipsToRegister, environment.ToString());
                            return;
                        }
                    }

                    string successMessage;

                    if (updatedExisting != null && createdIps.Count > 0)
                    {
                        successMessage = "IP-адрес обновлен, новые IP-адреса добавлены";
                    }
                    else if (updatedExisting != null)
                    {
                        successMessage = "IP-адрес успешно обновлен";
                    }
                    else
                    {
                        successMessage = createdIps.Count == 1
                            ? "IP-адрес успешно добавлен"
                            : "IP-адреса успешно добавлены";
                    }

                    MessageBox.Show(successMessage, "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
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

        private void BtnAddIp_Click(object sender, EventArgs e)
        {
            var ipAddress = txtIPAddress.Text.Trim();

            if (string.IsNullOrEmpty(ipAddress))
            {
                MessageBox.Show("Введите IP-адрес, прежде чем добавлять его в список", "Подсказка",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!IPAddress.TryParse(ipAddress, out var parsed) || parsed.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                MessageBox.Show("Введите корректный IPv4-адрес", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsIpAlreadyListed(ipAddress))
            {
                MessageBox.Show("Такой IP-адрес уже есть в таблице", "Подсказка",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!CheckExistingIP(ipAddress, adjustFormState: true, showExistingMessage: true))
            {
                return;
            }

            dgvIpAddresses.Rows.Add(ipAddress, GetEnvironmentDisplayName(GetEnvironmentType()));
            txtIPAddress.Clear();
            txtIPAddress.Focus();
            UpdateIpListControls();
        }

        private void BtnRemoveIp_Click(object sender, EventArgs e)
        {
            if (dgvIpAddresses.SelectedRows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow row in dgvIpAddresses.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    dgvIpAddresses.Rows.Remove(row);
                }
            }

            UpdateIpListControls();
        }

        private void DgvIpAddresses_SelectionChanged(object sender, EventArgs e)
        {
            UpdateIpListControls();
        }

        private bool EnsurePendingIpCaptured()
        {
            if (_isEditMode || _existingIpMatch != null)
            {
                return true;
            }

            var ipAddress = txtIPAddress.Text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
            {
                return true;
            }

            var previousCount = dgvIpAddresses.Rows.Count;
            BtnAddIp_Click(this, EventArgs.Empty);
            return dgvIpAddresses.Rows.Count > previousCount;
        }

        private List<string> GetIpEntries()
        {
            return dgvIpAddresses.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.Cells[colIpAddress.Index].Value?.ToString())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private bool IsIpAlreadyListed(string ipAddress)
        {
            return dgvIpAddresses.Rows
                .Cast<DataGridViewRow>()
                .Any(r => string.Equals(r.Cells[colIpAddress.Index].Value?.ToString(), ipAddress, StringComparison.OrdinalIgnoreCase));
        }

        private void UpdateIpListControls()
        {
            if (_isEditMode)
            {
                btnRemoveIp.Enabled = false;
                return;
            }

            btnRemoveIp.Enabled = dgvIpAddresses.SelectedRows.Count > 0;
        }

        private void RefreshPendingIpEnvironmentColumn()
        {
            if (_isEditMode || colEnvironment.Index < 0 || colEnvironment.Index >= dgvIpAddresses.Columns.Count)
            {
                return;
            }

            var label = GetEnvironmentDisplayName(GetEnvironmentType());

            foreach (DataGridViewRow row in dgvIpAddresses.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells[colEnvironment.Index].Value = label;
                }
            }
        }

        private void ToggleMultiIpControls(bool enabled)
        {
            lblIpList.Visible = enabled;
            dgvIpAddresses.Visible = enabled;
            btnAddIp.Visible = enabled;
            btnRemoveIp.Visible = enabled;

            lblIpList.Enabled = enabled;
            dgvIpAddresses.Enabled = enabled;
            btnAddIp.Enabled = enabled;
            btnRemoveIp.Enabled = enabled && dgvIpAddresses.SelectedRows.Count > 0;
            UpdateIpListControls();
            RefreshPendingIpEnvironmentColumn();
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

            if (!chkCombined.Checked && _existingIpMatch != null)
            {
                if ((_currentIpExistsInProduction && !_currentIpExistsInTest) ||
                    (_currentIpExistsInTest && !_currentIpExistsInProduction))
                {
                    chkCombined.Enabled = false;
                }
            }

            RefreshPendingIpEnvironmentColumn();
        }

        private void EnvironmentCheckBoxChanged(object sender, EventArgs e)
        {
            if (_isUpdatingCombinedState)
            {
                return;
            }

            if (_existingIpMatch != null)
            {
                if (_currentIpExistsInProduction && !_currentIpExistsInTest && chkTest.Checked)
                {
                    PromoteDuplicateToCombined();
                    return;
                }

                if (_currentIpExistsInTest && !_currentIpExistsInProduction && chkProduction.Checked)
                {
                    PromoteDuplicateToCombined();
                    return;
                }
            }

            var shouldCombine = chkProduction.Checked && chkTest.Checked;
            SetCombinedState(shouldCombine, false);
            RefreshPendingIpEnvironmentColumn();
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

            RefreshPendingIpEnvironmentColumn();
        }

        private void PromoteDuplicateToCombined()
        {
            _isUpdatingCombinedState = true;
            try
            {
                chkProduction.Checked = true;
                chkTest.Checked = true;
            }
            finally
            {
                _isUpdatingCombinedState = false;
            }

            SetCombinedState(true, true);
            chkCombined.Enabled = true;
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

        private async System.Threading.Tasks.Task RegisterInNamenAsync(List<IPAddressInfo> ipAddresses, string environment)
        {
            try
            {
                var namenService = new Services.NamenService();
                var failedRegistrations = new List<string>();
                var createdRequests = new List<string>();

                foreach (var ipInfo in ipAddresses)
                {
                    var result = await namenService.RegisterIPInNamen(ipInfo.IPAddress, environment);

                    if (result.Success)
                    {
                        _dbManager.UpdateNamenRegistration(ipInfo.Id, true, result.RequestNumber);
                        createdRequests.Add($"{ipInfo.IPAddress} → {result.RequestNumber}");
                    }
                    else
                    {
                        failedRegistrations.Add(ipInfo.IPAddress);
                    }
                }

                if (createdRequests.Count > 0)
                {
                    MessageBox.Show("IP успешно зарегистрированы в namen:\n" + string.Join(Environment.NewLine, createdRequests),
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (failedRegistrations.Count > 0)
                {
                    MessageBox.Show("Не удалось зарегистрировать следующие IP в namen:\n" + string.Join(Environment.NewLine, failedRegistrations),
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                DialogResult = DialogResult.OK;
                Close();
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

            var ipAddresses = _dbManager.GetIPAddressesForSystem(system.Id);

            if (ipAddresses.Count == 0)
            {
                infoLines.Add("IP-адреса: отсутствуют");
            }
            else
            {
                foreach (var summary in BuildEnvironmentSummary(ipAddresses))
                {
                    infoLines.Add(summary);
                }
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

        private IEnumerable<string> BuildEnvironmentSummary(IEnumerable<IPAddressInfo> ipAddresses)
        {
            var grouped = ipAddresses
                .GroupBy(ip => ip.Environment)
                .OrderBy(group => group.Key);

            foreach (var group in grouped)
            {
                var label = GetEnvironmentDisplayName(group.Key);
                var addresses = group
                    .Select(ip => ip.IPAddress)
                    .OrderBy(address => address)
                    .ToList();

                yield return $"{label}: {addresses.Count} IP ({string.Join(", ", addresses)})";
            }
        }

        private static string GetEnvironmentDisplayName(EnvironmentType environment)
        {
            switch (environment)
            {
                case EnvironmentType.Test:
                    return "Тестовый контур";
                case EnvironmentType.Production:
                    return "Промышленный контур";
                case EnvironmentType.Both:
                    return "Общий контур";
                default:
                    return environment.ToString();
            }
        }
    }
}
