using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using IPWhiteListManager.Data;
using IPWhiteListManager.Models;
using IPWhiteListManager.Services;

namespace IPWhiteListManager.Forms
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly NamenService _namenService;
        private BindingList<IPAddressInfo> _ipAddresses;
        private List<SystemInfo> _systems;
        private IPAddressInfo _selectedIP;

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
            this.dgvIPAddresses.SelectionChanged += DgvIPAddresses_SelectionChanged;
            this.txtFilter.KeyPress += TxtFilter_KeyPress;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        private void LoadData()
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
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
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
                _selectedIP = null;
                return;
            }

            _selectedIP = (IPAddressInfo)dgvIPAddresses.SelectedRows[0].DataBoundItem;
            var system = _systems.FirstOrDefault(s => s.Id == _selectedIP.SystemId);

            lblSelectedItem.Text = $"{_selectedIP.SystemName} ({_selectedIP.IPAddress}) - {_selectedIP.Environment}";

            var techInfo = new List<string>();

            if (system != null)
            {
                if (!string.IsNullOrEmpty(system.CuratorName))
                    techInfo.Add($"Куратор: {system.CuratorName}");

                if (!string.IsNullOrEmpty(system.CuratorEmail))
                    techInfo.Add($"Email куратора: {system.CuratorEmail}");

                if (!string.IsNullOrEmpty(system.OwnerName))
                    techInfo.Add($"Владелец системы: {system.OwnerName}");

                if (!string.IsNullOrEmpty(system.OwnerEmail))
                    techInfo.Add($"Email владельца: {system.OwnerEmail}");

                if (!string.IsNullOrEmpty(system.TechnicalSpecialistName))
                    techInfo.Add($"Технический специалист: {system.TechnicalSpecialistName}");

                if (!string.IsNullOrEmpty(system.TechnicalSpecialistEmail))
                    techInfo.Add($"Email специалиста: {system.TechnicalSpecialistEmail}");

                if (!string.IsNullOrEmpty(system.Description))
                    techInfo.Add($"Описание: {system.Description}");

                techInfo.Add($"Объединенные контуры: {(system.IsTestProductionCombined ? "Да" : "Нет")}");
            }

            techInfo.Add($"Статус в namen: {(_selectedIP.IsRegisteredInNamen ? "✓ Зарегистрирован" : "✗ Не зарегистрирован")}");

            if (!string.IsNullOrEmpty(_selectedIP.NamenRequestNumber))
                techInfo.Add($"№ заявки в namen: {_selectedIP.NamenRequestNumber}");

            techInfo.Add($"Дата добавления: {_selectedIP.RegistrationDate:dd.MM.yyyy HH:mm:ss}");

            txtTechInfo.Text = string.Join(Environment.NewLine, techInfo);

            // Обновление состояния кнопки
            btnRegisterNamen.Enabled = !_selectedIP.IsRegisteredInNamen && string.IsNullOrEmpty(_selectedIP.NamenRequestNumber);
        }

        private void BtnAddIP_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddIPForm(_dbManager))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    ApplyFilters();
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
    }
}