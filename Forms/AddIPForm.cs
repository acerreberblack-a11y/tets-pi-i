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

        public AddIPForm(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
            InitializeComponent();

            // Подписка на события
            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.txtIPAddress.TextChanged += TxtIPAddress_TextChanged;
            this.txtRequestNumber.TextChanged += TxtRequestNumber_TextChanged;

            LoadSystems();
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
            // Если введен номер заявки, отключаем автоматическую регистрацию
            if (!string.IsNullOrEmpty(txtRequestNumber.Text.Trim()))
            {
                chkRegisterNamen.Checked = false;
                chkRegisterNamen.Enabled = false;
            }
            else
            {
                chkRegisterNamen.Enabled = true;
            }
        }

        private void CheckExistingIP()
        {
            string ipAddress = txtIPAddress.Text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
                return;

            var existingIPs = _dbManager.FindIPAddresses(ipAddress);
            if (existingIPs.Any())
            {
                var firstIP = existingIPs.First();

                // Устанавливаем систему
                cmbSystem.Text = firstIP.SystemName;
                cmbSystem.Enabled = false;

                // Блокируем чекбоксы в зависимости от существующего окружения
                switch (firstIP.Environment)
                {
                    case EnvironmentType.Production:
                        chkProduction.Checked = true;
                        chkProduction.Enabled = false;
                        chkTest.Checked = false;
                        chkTest.Enabled = true;
                        break;
                    case EnvironmentType.Test:
                        chkProduction.Checked = false;
                        chkProduction.Enabled = true;
                        chkTest.Checked = true;
                        chkTest.Enabled = false;
                        break;
                    case EnvironmentType.Both:
                        chkProduction.Checked = true;
                        chkProduction.Enabled = false;
                        chkTest.Checked = true;
                        chkTest.Enabled = false;
                        break;
                }

                MessageBox.Show($"IP-адрес {ipAddress} уже существует в системе {firstIP.SystemName} ({firstIP.Environment}).\n" +
                               "Система и текущее окружение заблокированы для редактирования.",
                               "IP-адрес найден",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Разблокируем все поля если IP не найден
                cmbSystem.Enabled = true;
                chkProduction.Enabled = true;
                chkTest.Enabled = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Проверка введенных данных
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

            // Определение окружения
            var environment = GetEnvironmentType();

            // Получение или создание системы
            var systemId = GetOrCreateSystem(cmbSystem.Text.Trim());
            if (systemId == -1)
            {
                MessageBox.Show("Ошибка при создании системы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Создание и сохранение IP-адреса
            var ipInfo = new IPAddressInfo
            {
                SystemId = systemId,
                IPAddress = txtIPAddress.Text.Trim(),
                Environment = environment,
                IsRegisteredInNamen = !string.IsNullOrEmpty(txtRequestNumber.Text.Trim()) || chkRegisterNamen.Checked,
                NamenRequestNumber = string.IsNullOrEmpty(txtRequestNumber.Text.Trim()) ? null : txtRequestNumber.Text.Trim()
            };

            try
            {
                var ipId = _dbManager.AddIPAddress(ipInfo);

                // Регистрация в namen если отмечено и нет ручного номера заявки
                if (chkRegisterNamen.Checked && string.IsNullOrEmpty(txtRequestNumber.Text.Trim()))
                {
                    RegisterInNamen(ipId, ipInfo.IPAddress, environment.ToString());
                }
                else
                {
                    MessageBox.Show("IP-адрес успешно добавлен", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении IP-адреса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetOrCreateSystem(string systemName)
        {
            // Поиск существующей системы
            var systems = _dbManager.GetAllSystems();
            var existingSystem = systems.FirstOrDefault(s => s.SystemName.Equals(systemName, StringComparison.OrdinalIgnoreCase));

            if (existingSystem != null)
            {
                return existingSystem.Id;
            }

            // Создание новой системы
            var newSystem = new SystemInfo
            {
                SystemName = systemName,
                Description = "Автоматически создана при добавлении IP",
                IsTestProductionCombined = chkProduction.Checked && chkTest.Checked,
                CuratorName = "",
                CuratorEmail = ""
            };

            return _dbManager.AddSystem(newSystem);
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
    }
}