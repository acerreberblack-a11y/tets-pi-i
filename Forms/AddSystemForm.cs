using System;
using System.ComponentModel;
using System.Net.Mail;
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
            btnCopyOwnerToTech.Click += BtnCopyOwnerToTech_Click;

            txtSystemName.Validating += RequiredField_Validating;
            txtOwnerName.Validating += RequiredField_Validating;
            txtOwnerEmail.Validating += OwnerEmail_Validating;
            txtTechnicalName.Validating += RequiredField_Validating;
            txtTechnicalEmail.Validating += TechnicalEmail_Validating;

            toolTip.SetToolTip(txtSystemName, "Официальное наименование информационной системы");
            toolTip.SetToolTip(txtDescription, "Краткое описание назначения или ключевых особенностей ИС");
            toolTip.SetToolTip(txtOwnerEmail, "Рабочая почта владельца системы");
            toolTip.SetToolTip(txtTechnicalEmail, "Почта ответственного технического специалиста");
            toolTip.SetToolTip(txtCuratorEmail, "Дополнительный контакт, например, куратора из ТП");
            toolTip.SetToolTip(btnCopyOwnerToTech, "Если владелец и технический специалист совпадают, заполните поля автоматически");

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
            txtCuratorName.Text = system.CuratorName ?? string.Empty;
            txtCuratorEmail.Text = system.CuratorEmail ?? string.Empty;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (!ValidateForm())
            {
                MessageBox.Show("Проверьте корректность заполнения обязательных полей", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                CuratorName = string.IsNullOrWhiteSpace(txtCuratorName.Text) ? null : txtCuratorName.Text.Trim(),
                CuratorEmail = string.IsNullOrWhiteSpace(txtCuratorEmail.Text) ? null : txtCuratorEmail.Text.Trim()
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

        private void BtnCopyOwnerToTech_Click(object sender, EventArgs e)
        {
            txtTechnicalName.Text = txtOwnerName.Text;
            txtTechnicalEmail.Text = txtOwnerEmail.Text;
        }

        private void RequiredField_Validating(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    errorProvider.SetError(textBox, "Поле обязательно для заполнения");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError(textBox, string.Empty);
                }
            }
        }

        private void OwnerEmail_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmailField(txtOwnerEmail, true, e);
        }

        private void TechnicalEmail_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmailField(txtTechnicalEmail, true, e);
        }

        private bool ValidateForm()
        {
            var fields = new[]
            {
                txtSystemName,
                txtOwnerName,
                txtOwnerEmail,
                txtTechnicalName,
                txtTechnicalEmail
            };

            var allValid = true;

            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Text))
                {
                    errorProvider.SetError(field, "Поле обязательно для заполнения");
                    allValid = false;
                }
            }

            if (!ValidateEmail(txtOwnerEmail.Text))
            {
                errorProvider.SetError(txtOwnerEmail, "Введите корректный email владельца");
                allValid = false;
            }

            if (!ValidateEmail(txtTechnicalEmail.Text))
            {
                errorProvider.SetError(txtTechnicalEmail, "Введите корректный email технического специалиста");
                allValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtCuratorEmail.Text) && !ValidateEmail(txtCuratorEmail.Text))
            {
                errorProvider.SetError(txtCuratorEmail, "Введите корректный email куратора");
                allValid = false;
            }

            return allValid;
        }

        private void ValidateEmailField(TextBox textBox, bool required, CancelEventArgs e)
        {
            var value = textBox.Text.Trim();

            if (required && string.IsNullOrWhiteSpace(value))
            {
                errorProvider.SetError(textBox, "Поле обязательно для заполнения");
                e.Cancel = true;
                return;
            }

            if (!string.IsNullOrWhiteSpace(value) && !ValidateEmail(value))
            {
                errorProvider.SetError(textBox, "Введите корректный email");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(textBox, string.Empty);
            }
        }

        private static bool ValidateEmail(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
