using System;
using System.Windows.Forms;
using IPWhiteListManager.Data;
using IPWhiteListManager.Models;

namespace IPWhiteListManager.Forms
{
    public partial class AddSystemForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly bool _isEditMode;
        private readonly SystemInfo _systemToEdit;
        public SystemInfo CreatedSystem { get; private set; }

        public AddSystemForm(DatabaseManager dbManager, string systemName = null)
            : this(dbManager, null, systemName)
        {
        }

        public AddSystemForm(DatabaseManager dbManager, SystemInfo systemToEdit, string systemName = null)
        {
            _dbManager = dbManager;
            _systemToEdit = systemToEdit;
            _isEditMode = systemToEdit != null;

            InitializeComponent();

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            if (_isEditMode)
            {
                Text = "Редактирование ИС";
                CreatedSystem = systemToEdit;
                PopulateFields(systemToEdit);
            }
            else if (!string.IsNullOrWhiteSpace(systemName))
            {
                txtSystemName.Text = systemName;
            }
        }

        private void PopulateFields(SystemInfo system)
        {
            txtSystemName.Text = system.SystemName;
            txtDescription.Text = system.Description ?? string.Empty;
            txtOwnerName.Text = system.OwnerName ?? string.Empty;
            txtOwnerEmail.Text = system.OwnerEmail ?? string.Empty;
            txtTechnicalName.Text = system.TechnicalSpecialistName ?? string.Empty;
            txtTechnicalEmail.Text = system.TechnicalSpecialistEmail ?? string.Empty;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSystemName.Text))
            {
                MessageBox.Show("Введите наименование ИС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOwnerName.Text) || string.IsNullOrWhiteSpace(txtOwnerEmail.Text))
            {
                MessageBox.Show("Заполните данные владельца системы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTechnicalName.Text) || string.IsNullOrWhiteSpace(txtTechnicalEmail.Text))
            {
                MessageBox.Show("Заполните данные технического специалиста", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var system = new SystemInfo
            {
                SystemName = txtSystemName.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                OwnerName = txtOwnerName.Text.Trim(),
                OwnerEmail = txtOwnerEmail.Text.Trim(),
                TechnicalSpecialistName = txtTechnicalName.Text.Trim(),
                TechnicalSpecialistEmail = txtTechnicalEmail.Text.Trim(),
                IsTestProductionCombined = _systemToEdit?.IsTestProductionCombined ?? false,
                CuratorName = _systemToEdit?.CuratorName,
                CuratorEmail = _systemToEdit?.CuratorEmail
            };

            try
            {
                if (_isEditMode)
                {
                    system.Id = _systemToEdit.Id;
                    _dbManager.UpdateSystem(system);
                    CreatedSystem = _dbManager.GetSystemById(system.Id) ?? system;
                }
                else
                {
                    var id = _dbManager.AddSystem(system);
                    system.Id = id;
                    CreatedSystem = system;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить ИС: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
