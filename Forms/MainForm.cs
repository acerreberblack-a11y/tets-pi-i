using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IPWhiteListManager.Data;
using IPWhiteListManager.Models;
using IPWhiteListManager.Services;
using Microsoft.VisualBasic.FileIO;

namespace IPWhiteListManager.Forms
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly NamenService _namenService;
        private BindingList<IPAddressInfo> _ipAddresses;
        private List<SystemInfo> _systems;
        private IPAddressInfo _selectedIP;
        private bool _suppressFilterEvents;

        public MainForm()
        {
            InitializeComponent();

            _dbManager = new DatabaseManager();
            _namenService = new NamenService();
            _systems = new List<SystemInfo>();
            _ipAddresses = new BindingList<IPAddressInfo>();

            // Подписка на события
            this.btnSearch.Click += BtnSearch_Click;
            this.btnAddIP.Click += BtnAddIP_Click;
            this.btnAddSystem.Click += BtnAddSystem_Click;
            this.btnRegisterNamen.Click += BtnRegisterNamen_Click;
            this.btnEditIP.Click += BtnEditIP_Click;
            this.btnEditSystem.Click += BtnEditSystem_Click;
            this.btnDeleteIP.Click += BtnDeleteIP_Click;
            this.btnDeleteSystem.Click += BtnDeleteSystem_Click;
            this.btnExportCsv.Click += BtnExportCsv_Click;
            this.btnImportCsv.Click += BtnImportCsv_Click;
            this.dgvIPAddresses.SelectionChanged += DgvIPAddresses_SelectionChanged;
            this.txtFilter.KeyPress += TxtFilter_KeyPress;
            this.txtFilter.TextChanged += FilterControlChanged;
            this.cmbSystemFilter.SelectedIndexChanged += FilterControlChanged;
            this.cmbEnvironmentFilter.SelectedIndexChanged += FilterControlChanged;

            UpdateContactInfo(null);
            btnEditIP.Enabled = false;
            btnDeleteIP.Enabled = false;
            btnEditSystem.Enabled = false;
            btnDeleteSystem.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        private void LoadData()
        {
            _suppressFilterEvents = true;
            try
            {
                _systems = _dbManager.GetAllSystems();
                var ipAddresses = _dbManager.GetAllIPAddresses();

                // Заполнение фильтров
                cmbSystemFilter.Items.Clear();
                cmbSystemFilter.Items.Add("Все системы");
                foreach (var system in _systems)
                {
                    cmbSystemFilter.Items.Add(system.SystemName);
                }
                cmbSystemFilter.SelectedIndex = 0;

                cmbEnvironmentFilter.Items.Clear();
                cmbEnvironmentFilter.Items.Add("Все контуры");
                cmbEnvironmentFilter.Items.Add("Test");
                cmbEnvironmentFilter.Items.Add("Production");
                cmbEnvironmentFilter.Items.Add("Both");
                cmbEnvironmentFilter.SelectedIndex = 0;

                BindIpAddresses(ipAddresses);
            }
            finally
            {
                _suppressFilterEvents = false;
            }

            ApplyFilters();
            UpdateSystemButtonState();
        }

        private void SetupDataGridView()
        {
            dgvIPAddresses.AutoGenerateColumns = false;

            if (dgvIPAddresses.Columns.Count > 0)
            {
                return;
            }

            // Колонка ИС
            var systemColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SystemName",
                HeaderText = "ИС",
                Name = "SystemName",
                Width = 200
            };

            // Колонка Контур
            var environmentColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Environment",
                HeaderText = "Контур",
                Name = "Environment",
                Width = 100
            };

            // Колонка IP
            var ipColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IPAddress",
                HeaderText = "IP",
                Name = "IPAddress",
                Width = 120
            };

            dgvIPAddresses.Columns.AddRange(new DataGridViewColumn[] { systemColumn, environmentColumn, ipColumn });
        }

        private void BindIpAddresses(List<IPAddressInfo> ipAddresses)
        {
            SetupDataGridView();
            _ipAddresses = new BindingList<IPAddressInfo>(ipAddresses);
            dgvIPAddresses.DataSource = _ipAddresses;
            _selectedIP = null;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_suppressFilterEvents)
            {
                return;
            }

            var filtered = _dbManager.GetAllIPAddresses();

            // Фильтр по тексту
            if (!string.IsNullOrEmpty(txtFilter.Text))
            {
                var filterText = txtFilter.Text.ToLower();
                filtered = filtered.Where(ip =>
                    ip.IPAddress.ToLower().Contains(filterText) ||
                    ip.SystemName.ToLower().Contains(filterText)
                ).ToList();
            }

            // Фильтр по системе
            if (cmbSystemFilter.SelectedIndex > 0)
            {
                var selectedSystem = cmbSystemFilter.SelectedItem.ToString();
                filtered = filtered.Where(ip => ip.SystemName == selectedSystem).ToList();
            }

            // Фильтр по окружению
            if (cmbEnvironmentFilter.SelectedIndex > 0)
            {
                var selectedEnvironment = cmbEnvironmentFilter.SelectedItem.ToString();
                filtered = filtered.Where(ip => ip.Environment.ToString() == selectedEnvironment).ToList();
            }

            BindIpAddresses(filtered);
            UpdateSystemButtonState();
        }

        private void FilterControlChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            if (sender == cmbSystemFilter)
            {
                UpdateSystemButtonState();
            }
        }

        private void DgvIPAddresses_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTechnicalInfo();
        }

        private void UpdateTechnicalInfo()
        {
            if (dgvIPAddresses.SelectedRows.Count == 0)
            {
                lblSelectedItem.Text = "Выберите запись в таблице для просмотра деталей";
                txtTechInfo.Text = "";
                btnRegisterNamen.Enabled = false;
                btnEditIP.Enabled = false;
                btnDeleteIP.Enabled = false;
                _selectedIP = null;
                UpdateContactInfo(null);
                UpdateSystemButtonState();
                return;
            }

            _selectedIP = (IPAddressInfo)dgvIPAddresses.SelectedRows[0].DataBoundItem;
            var system = _systems.FirstOrDefault(s => s.Id == _selectedIP.SystemId);

            lblSelectedItem.Text = $"{_selectedIP.SystemName} ({_selectedIP.IPAddress}) - {_selectedIP.Environment}";
            btnEditIP.Enabled = true;
            btnDeleteIP.Enabled = true;
            UpdateContactInfo(system);

            var techInfo = new List<string>();

            if (system != null)
            {
                if (!string.IsNullOrEmpty(system.CuratorName))
                    techInfo.Add($"Куратор: {system.CuratorName}");

                if (!string.IsNullOrEmpty(system.CuratorEmail))
                    techInfo.Add($"Email куратора: {system.CuratorEmail}");

                if (!string.IsNullOrEmpty(system.Description))
                    techInfo.Add($"Описание: {system.Description}");

                techInfo.Add($"Объединенные контуры: {(system.IsTestProductionCombined ? "Да" : "Нет")}");

                foreach (var summary in BuildEnvironmentSummary(system))
                {
                    techInfo.Add(summary);
                }
            }

            techInfo.Add($"Статус в namen: {(_selectedIP.IsRegisteredInNamen ? "✓ Зарегистрирован" : "✗ Не зарегистрирован")}");

            if (!string.IsNullOrEmpty(_selectedIP.NamenRequestNumber))
                techInfo.Add($"№ заявки в namen: {_selectedIP.NamenRequestNumber}");

            techInfo.Add($"Дата добавления: {_selectedIP.RegistrationDate:dd.MM.yyyy HH:mm:ss}");

            txtTechInfo.Text = string.Join(Environment.NewLine, techInfo);

            // Обновление состояния кнопки
            btnRegisterNamen.Enabled = !_selectedIP.IsRegisteredInNamen && string.IsNullOrEmpty(_selectedIP.NamenRequestNumber);
            UpdateSystemButtonState();
        }

        private void BtnAddIP_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddIPForm(_dbManager))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    ApplyFilters();
                    UpdateSystemButtonState();
                }
            }
        }

        private void BtnAddSystem_Click(object sender, EventArgs e)
        {
            using (var addSystemForm = new AddSystemForm(_dbManager))
            {
                if (addSystemForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    ApplyFilters();
                    UpdateSystemButtonState();
                }
            }
        }

        private void BtnRegisterNamen_Click(object sender, EventArgs e)
        {
            if (_selectedIP == null)
            {
                MessageBox.Show("Выберите IP-адрес из таблицы", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_selectedIP.IsRegisteredInNamen || !string.IsNullOrEmpty(_selectedIP.NamenRequestNumber))
            {
                MessageBox.Show("Этот IP-адрес уже зарегистрирован в namen или имеет номер заявки", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RegisterInNamen(_selectedIP.Id, _selectedIP.IPAddress, _selectedIP.Environment.ToString());
        }

        private async void RegisterInNamen(int ipAddressId, string ipAddress, string environment)
        {
            try
            {
                btnRegisterNamen.Enabled = false;
                btnRegisterNamen.Text = "Регистрация...";

                var result = await _namenService.RegisterIPInNamen(ipAddress, environment);

                if (result.Success)
                {
                    _dbManager.UpdateNamenRegistration(ipAddressId, true, result.RequestNumber);
                    MessageBox.Show($"IP {ipAddress} успешно зарегистрирован в namen\nНомер заявки: {result.RequestNumber}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ApplyFilters();
                    UpdateSystemButtonState();
                }
                else
                {
                    MessageBox.Show($"Не удалось зарегистрировать IP {ipAddress} в namen", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnRegisterNamen.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации в namen: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRegisterNamen.Enabled = true;
            }
            finally
            {
                btnRegisterNamen.Text = "Зарегистрировать в namen";
            }
        }

        private void TxtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ApplyFilters();
                e.Handled = true;
            }
        }

        private void BtnEditIP_Click(object sender, EventArgs e)
        {
            if (_selectedIP == null)
            {
                MessageBox.Show("Выберите IP-адрес для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var system = _systems.FirstOrDefault(s => s.Id == _selectedIP.SystemId);
            var ipId = _selectedIP.Id;

            using (var editForm = new AddIPForm(_dbManager, _selectedIP, system))
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadData();
                    SelectIpRow(ipId);
                    UpdateSystemButtonState();
                }
            }
        }

        private void BtnEditSystem_Click(object sender, EventArgs e)
        {
            SystemInfo systemToEdit = null;

            if (_selectedIP != null)
            {
                systemToEdit = _systems.FirstOrDefault(s => s.Id == _selectedIP.SystemId);
            }

            if (systemToEdit == null && cmbSystemFilter.SelectedIndex > 0)
            {
                var selectedName = cmbSystemFilter.SelectedItem.ToString();
                systemToEdit = _systems.FirstOrDefault(s => s.SystemName.Equals(selectedName, StringComparison.OrdinalIgnoreCase));
            }

            if (systemToEdit == null)
            {
                MessageBox.Show("Выберите ИС в таблице или фильтре", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedIpId = _selectedIP?.Id;

            using (var editForm = new AddSystemForm(_dbManager, systemToEdit))
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    var updatedSystem = editForm.CreatedSystem ?? _dbManager.GetSystemById(systemToEdit.Id) ?? systemToEdit;

                    LoadData();
                    SelectSystemFilter(updatedSystem.SystemName);

                    if (selectedIpId.HasValue)
                    {
                        SelectIpRow(selectedIpId.Value);
                    }
                    UpdateSystemButtonState();
                }
            }
        }

        private void BtnDeleteIP_Click(object sender, EventArgs e)
        {
            if (_selectedIP == null)
            {
                MessageBox.Show("Выберите IP-адрес для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmation = MessageBox.Show(
                $"Удалить IP {_selectedIP.IPAddress} ({_selectedIP.Environment})?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            if (_dbManager.DeleteIPAddress(_selectedIP.Id))
            {
                MessageBox.Show("IP-адрес удален", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ApplyFilters();
            }
            else
            {
                MessageBox.Show("Не удалось удалить IP-адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateSystemButtonState();
        }

        private void UpdateContactInfo(SystemInfo system)
        {
            lblOwnerNameValue.Text = string.IsNullOrWhiteSpace(system?.OwnerName) ? "-" : system.OwnerName;
            lblOwnerEmailValue.Text = string.IsNullOrWhiteSpace(system?.OwnerEmail) ? "-" : system.OwnerEmail;
            lblTechNameValue.Text = string.IsNullOrWhiteSpace(system?.TechnicalSpecialistName) ? "-" : system.TechnicalSpecialistName;
            lblTechEmailValue.Text = string.IsNullOrWhiteSpace(system?.TechnicalSpecialistEmail) ? "-" : system.TechnicalSpecialistEmail;
        }

        private IEnumerable<string> BuildEnvironmentSummary(SystemInfo system)
        {
            if (system == null)
            {
                yield break;
            }

            var ipAddresses = _dbManager.GetIPAddressesForSystem(system.Id);

            if (ipAddresses.Count == 0)
            {
                yield return "IP-адреса по системе: отсутствуют";
                yield break;
            }

            foreach (var group in ipAddresses
                         .GroupBy(ip => ip.Environment)
                         .OrderBy(group => group.Key))
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

        private void SelectIpRow(int ipId)
        {
            if (dgvIPAddresses.Rows.Count == 0)
            {
                return;
            }

            dgvIPAddresses.ClearSelection();

            foreach (DataGridViewRow row in dgvIPAddresses.Rows)
            {
                if (row.DataBoundItem is IPAddressInfo ip && ip.Id == ipId)
                {
                    row.Selected = true;
                    if (row.Cells.Count > 0)
                    {
                        dgvIPAddresses.CurrentCell = row.Cells[0];
                    }

                    if (row.Index >= 0 && row.Index < dgvIPAddresses.RowCount)
                    {
                        dgvIPAddresses.FirstDisplayedScrollingRowIndex = row.Index;
                    }
                    break;
                }
            }
        }

        private void SelectSystemFilter(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                return;
            }

            for (int i = 0; i < cmbSystemFilter.Items.Count; i++)
            {
                if (cmbSystemFilter.Items[i].ToString().Equals(systemName, StringComparison.OrdinalIgnoreCase))
                {
                    cmbSystemFilter.SelectedIndex = i;
                    return;
                }
            }
        }

        private void BtnDeleteSystem_Click(object sender, EventArgs e)
        {
            var systemToDelete = ResolveSystemFromContext();

            if (systemToDelete == null)
            {
                MessageBox.Show("Выберите ИС для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmation = MessageBox.Show(
                $"Удалить ИС '{systemToDelete.SystemName}' и связанные IP?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            if (_dbManager.DeleteSystem(systemToDelete.Id))
            {
                MessageBox.Show("Информационная система удалена", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                cmbSystemFilter.SelectedIndex = 0;
                ApplyFilters();
            }
            else
            {
                MessageBox.Show("Не удалось удалить ИС", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateSystemButtonState();
        }

        private SystemInfo ResolveSystemFromContext()
        {
            if (_selectedIP != null)
            {
                var system = _systems.FirstOrDefault(s => s.Id == _selectedIP.SystemId);
                if (system != null)
                {
                    return system;
                }
            }

            if (cmbSystemFilter.SelectedIndex > 0)
            {
                var selectedName = cmbSystemFilter.SelectedItem.ToString();
                return _systems.FirstOrDefault(s => s.SystemName.Equals(selectedName, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }

        private void UpdateSystemButtonState()
        {
            var systemAvailable = ResolveSystemFromContext() != null;
            btnEditSystem.Enabled = systemAvailable;
            btnDeleteSystem.Enabled = systemAvailable;
        }

        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            if (_ipAddresses == null || _ipAddresses.Count == 0)
            {
                MessageBox.Show("Нет данных для выгрузки", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV файлы (*.csv)|*.csv";
                dialog.FileName = $"ip_export_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        ExportCurrentDataToCsv(dialog.FileName);
                        MessageBox.Show("Данные успешно сохранены", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportCurrentDataToCsv(string filePath)
        {
            var builder = new StringBuilder();
            builder.AppendLine("SystemName;Environment;IPAddress;IsRegisteredInNamen;NamenRequestNumber;RegistrationDate;OwnerName;OwnerEmail;TechnicalSpecialistName;TechnicalSpecialistEmail");

            foreach (var ip in _ipAddresses)
            {
                var system = _systems.FirstOrDefault(s => s.Id == ip.SystemId);

                builder.AppendLine(string.Join(";", new[]
                {
                    CsvValue(ip.SystemName),
                    CsvValue(ip.Environment.ToString()),
                    CsvValue(ip.IPAddress),
                    CsvValue(ip.IsRegisteredInNamen ? "true" : "false"),
                    CsvValue(ip.NamenRequestNumber),
                    CsvValue(ip.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss")),
                    CsvValue(system?.OwnerName),
                    CsvValue(system?.OwnerEmail),
                    CsvValue(system?.TechnicalSpecialistName),
                    CsvValue(system?.TechnicalSpecialistEmail)
                }));
            }

            File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
        }

        private static string CsvValue(string value)
        {
            var sanitized = (value ?? string.Empty).Replace("\"", "\"\"");
            return $"\"{sanitized}\"";
        }

        private void BtnImportCsv_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var summary = ImportFromCsv(dialog.FileName);
                        LoadData();

                        var message = new StringBuilder();
                        message.AppendLine("Импорт завершен.");
                        message.AppendLine($"Добавлено ИС: {summary.SystemsCreated}, обновлено ИС: {summary.SystemsUpdated}");
                        message.AppendLine($"Добавлено IP: {summary.IPsCreated}, обновлено IP: {summary.IPsUpdated}");

                        if (summary.SkippedRows > 0)
                        {
                            message.AppendLine($"Пропущено строк: {summary.SkippedRows}");
                        }

                        if (summary.Errors.Count > 0)
                        {
                            message.AppendLine();
                            message.AppendLine("Ошибки:");
                            foreach (var error in summary.Errors.Take(5))
                            {
                                message.AppendLine(" - " + error);
                            }

                            if (summary.Errors.Count > 5)
                            {
                                message.AppendLine($"... и ещё {summary.Errors.Count - 5} строк(и).");
                            }
                        }

                        MessageBox.Show(message.ToString(), "Импорт CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private ImportSummary ImportFromCsv(string filePath)
        {
            var summary = new ImportSummary();

            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                parser.HasFieldsEnclosedInQuotes = true;

                if (parser.EndOfData)
                {
                    return summary;
                }

                var rawHeaders = parser.ReadFields();
                if (rawHeaders == null)
                {
                    return summary;
                }

                var headers = new string[rawHeaders.Length];
                for (var i = 0; i < rawHeaders.Length; i++)
                {
                    var value = rawHeaders[i] ?? string.Empty;
                    headers[i] = value.Trim().Trim((char)0xFEFF);
                }

                var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (var i = 0; i < headers.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(headers[i]) && !headerMap.ContainsKey(headers[i]))
                    {
                        headerMap.Add(headers[i], i);
                    }
                }

                var requiredColumns = new[] { "SystemName", "Environment", "IPAddress" };
                foreach (var column in requiredColumns)
                {
                    if (!headerMap.ContainsKey(column))
                    {
                        throw new InvalidOperationException($"В файле отсутствует обязательный столбец '{column}'.");
                    }
                }

                var lineNumber = 1; // header already read
                while (!parser.EndOfData)
                {
                    string[] fields;
                    try
                    {
                        fields = parser.ReadFields();
                    }
                    catch (MalformedLineException ex)
                    {
                        summary.SkippedRows++;
                        summary.Errors.Add($"Строка {lineNumber + 1}: {ex.Message}");
                        continue;
                    }

                    lineNumber++;

                    if (fields == null || fields.Length == 0)
                    {
                        continue;
                    }

                    try
                    {
                        ProcessImportRow(fields, headerMap, summary);
                    }
                    catch (Exception ex)
                    {
                        summary.SkippedRows++;
                        summary.Errors.Add($"Строка {lineNumber}: {ex.Message}");
                    }
                }
            }

            return summary;
        }

        private void ProcessImportRow(string[] fields, Dictionary<string, int> headerMap, ImportSummary summary)
        {
            var systemName = GetFieldValue(headerMap, fields, "SystemName");
            var environmentText = GetFieldValue(headerMap, fields, "Environment");
            var ipAddress = GetFieldValue(headerMap, fields, "IPAddress");

            if (string.IsNullOrWhiteSpace(systemName) || string.IsNullOrWhiteSpace(environmentText) || string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new InvalidOperationException("Не заполнены обязательные поля SystemName, Environment или IPAddress.");
            }

            if (!Enum.TryParse(environmentText, true, out EnvironmentType environment))
            {
                throw new InvalidOperationException($"Неизвестное значение контура: '{environmentText}'.");
            }

            var ownerName = GetFieldValue(headerMap, fields, "OwnerName");
            var ownerEmail = GetFieldValue(headerMap, fields, "OwnerEmail");
            var techName = GetFieldValue(headerMap, fields, "TechnicalSpecialistName");
            var techEmail = GetFieldValue(headerMap, fields, "TechnicalSpecialistEmail");
            var curatorName = GetFieldValue(headerMap, fields, "CuratorName");
            var curatorEmail = GetFieldValue(headerMap, fields, "CuratorEmail");
            var description = GetFieldValue(headerMap, fields, "Description");
            var isRegisteredText = GetFieldValue(headerMap, fields, "IsRegisteredInNamen");
            var requestNumber = GetFieldValue(headerMap, fields, "NamenRequestNumber");
            var registrationDateText = GetFieldValue(headerMap, fields, "RegistrationDate");

            var isRegistered = ParseBoolean(isRegisteredText);
            var registrationDate = ParseDateTime(registrationDateText);

            var system = _dbManager.FindSystemByName(systemName);
            if (system == null)
            {
                system = new SystemInfo
                {
                    SystemName = systemName,
                    Description = string.IsNullOrWhiteSpace(description) ? null : description,
                    CuratorName = string.IsNullOrWhiteSpace(curatorName) ? null : curatorName,
                    CuratorEmail = string.IsNullOrWhiteSpace(curatorEmail) ? null : curatorEmail,
                    OwnerName = string.IsNullOrWhiteSpace(ownerName) ? null : ownerName,
                    OwnerEmail = string.IsNullOrWhiteSpace(ownerEmail) ? null : ownerEmail,
                    TechnicalSpecialistName = string.IsNullOrWhiteSpace(techName) ? null : techName,
                    TechnicalSpecialistEmail = string.IsNullOrWhiteSpace(techEmail) ? null : techEmail,
                    IsTestProductionCombined = environment == EnvironmentType.Both
                };

                system.Id = _dbManager.AddSystem(system);
                summary.SystemsCreated++;
            }
            else
            {
                var updated = false;

                if (!string.IsNullOrWhiteSpace(description) && !string.Equals(description, system.Description))
                {
                    system.Description = description;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(curatorName) && !string.Equals(curatorName, system.CuratorName))
                {
                    system.CuratorName = curatorName;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(curatorEmail) && !string.Equals(curatorEmail, system.CuratorEmail))
                {
                    system.CuratorEmail = curatorEmail;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(ownerName) && !string.Equals(ownerName, system.OwnerName))
                {
                    system.OwnerName = ownerName;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(ownerEmail) && !string.Equals(ownerEmail, system.OwnerEmail))
                {
                    system.OwnerEmail = ownerEmail;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(techName) && !string.Equals(techName, system.TechnicalSpecialistName))
                {
                    system.TechnicalSpecialistName = techName;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(techEmail) && !string.Equals(techEmail, system.TechnicalSpecialistEmail))
                {
                    system.TechnicalSpecialistEmail = techEmail;
                    updated = true;
                }

                if (environment == EnvironmentType.Both && !system.IsTestProductionCombined)
                {
                    system.IsTestProductionCombined = true;
                    updated = true;
                }

                if (updated)
                {
                    _dbManager.UpdateSystem(system);
                    summary.SystemsUpdated++;
                }
            }

            var existingEntries = _dbManager.FindIPAddresses(ipAddress);
            var existingIp = existingEntries.FirstOrDefault(x => x.Environment == environment);

            if (existingIp == null)
            {
                var newIp = new IPAddressInfo
                {
                    SystemId = system.Id,
                    IPAddress = ipAddress,
                    Environment = environment,
                    IsRegisteredInNamen = isRegistered,
                    NamenRequestNumber = string.IsNullOrWhiteSpace(requestNumber) ? null : requestNumber
                };

                _dbManager.AddIPAddress(newIp, registrationDate);
                summary.IPsCreated++;
            }
            else
            {
                existingIp.SystemId = system.Id;
                existingIp.Environment = environment;
                existingIp.IsRegisteredInNamen = isRegistered;
                existingIp.NamenRequestNumber = string.IsNullOrWhiteSpace(requestNumber) ? null : requestNumber;

                _dbManager.UpdateIPAddress(existingIp, registrationDate);
                summary.IPsUpdated++;
            }
        }

        private static string GetFieldValue(Dictionary<string, int> headerMap, string[] fields, string column)
        {
            if (!headerMap.TryGetValue(column, out var index))
            {
                return null;
            }

            if (index < 0 || index >= fields.Length)
            {
                return null;
            }

            var value = fields[index];
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool ParseBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var normalized = value.Trim();
            return normalized.Equals("1") ||
                   normalized.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   normalized.Equals("да", StringComparison.OrdinalIgnoreCase) ||
                   normalized.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private static DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var formats = new[]
            {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd",
                "dd.MM.yyyy HH:mm:ss",
                "dd.MM.yyyy"
            };

            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var exact))
            {
                return exact;
            }

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed) ||
                DateTime.TryParse(value, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out parsed))
            {
                return parsed;
            }

            return null;
        }

        private class ImportSummary
        {
            public int SystemsCreated { get; set; }
            public int SystemsUpdated { get; set; }
            public int IPsCreated { get; set; }
            public int IPsUpdated { get; set; }
            public int SkippedRows { get; set; }
            public List<string> Errors { get; } = new List<string>();
        }
    }
}
