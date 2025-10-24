namespace IPWhiteListManager.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvIPAddresses = new System.Windows.Forms.DataGridView();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.cmbSystemFilter = new System.Windows.Forms.ComboBox();
            this.cmbEnvironmentFilter = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAddIP = new System.Windows.Forms.Button();
            this.btnRegisterNamen = new System.Windows.Forms.Button();
            this.txtTechInfo = new System.Windows.Forms.TextBox();
            this.lblSelectedItem = new System.Windows.Forms.Label();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.panelTechInfo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIPAddresses)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.panelTechInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvIPAddresses
            // 
            this.dgvIPAddresses.AllowUserToAddRows = false;
            this.dgvIPAddresses.AllowUserToDeleteRows = false;
            this.dgvIPAddresses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIPAddresses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIPAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIPAddresses.Location = new System.Drawing.Point(0, 60);
            this.dgvIPAddresses.Name = "dgvIPAddresses";
            this.dgvIPAddresses.ReadOnly = true;
            this.dgvIPAddresses.RowHeadersVisible = false;
            this.dgvIPAddresses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIPAddresses.Size = new System.Drawing.Size(1000, 410);
            this.dgvIPAddresses.TabIndex = 0;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(33, 12);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(181, 20);
            this.txtFilter.TabIndex = 1;
            // 
            // cmbSystemFilter
            // 
            this.cmbSystemFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemFilter.FormattingEnabled = true;
            this.cmbSystemFilter.Location = new System.Drawing.Point(240, 12);
            this.cmbSystemFilter.Name = "cmbSystemFilter";
            this.cmbSystemFilter.Size = new System.Drawing.Size(242, 21);
            this.cmbSystemFilter.TabIndex = 2;
            // 
            // cmbEnvironmentFilter
            // 
            this.cmbEnvironmentFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEnvironmentFilter.FormattingEnabled = true;
            this.cmbEnvironmentFilter.Location = new System.Drawing.Point(536, 12);
            this.cmbEnvironmentFilter.Name = "cmbEnvironmentFilter";
            this.cmbEnvironmentFilter.Size = new System.Drawing.Size(224, 21);
            this.cmbEnvironmentFilter.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(797, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 25);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnAddIP
            // 
            this.btnAddIP.Location = new System.Drawing.Point(887, 9);
            this.btnAddIP.Name = "btnAddIP";
            this.btnAddIP.Size = new System.Drawing.Size(100, 25);
            this.btnAddIP.TabIndex = 5;
            this.btnAddIP.Text = "Добавить IP";
            this.btnAddIP.UseVisualStyleBackColor = true;
            // 
            // btnRegisterNamen
            // 
            this.btnRegisterNamen.BackColor = System.Drawing.Color.LightGreen;
            this.btnRegisterNamen.Enabled = false;
            this.btnRegisterNamen.Location = new System.Drawing.Point(10, 145);
            this.btnRegisterNamen.Name = "btnRegisterNamen";
            this.btnRegisterNamen.Size = new System.Drawing.Size(180, 25);
            this.btnRegisterNamen.TabIndex = 6;
            this.btnRegisterNamen.Text = "Зарегистрировать в naumen";
            this.btnRegisterNamen.UseVisualStyleBackColor = false;
            // 
            // txtTechInfo
            // 
            this.txtTechInfo.BackColor = System.Drawing.Color.AliceBlue;
            this.txtTechInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTechInfo.Location = new System.Drawing.Point(10, 35);
            this.txtTechInfo.Multiline = true;
            this.txtTechInfo.Name = "txtTechInfo";
            this.txtTechInfo.ReadOnly = true;
            this.txtTechInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTechInfo.Size = new System.Drawing.Size(750, 100);
            this.txtTechInfo.TabIndex = 7;
            // 
            // lblSelectedItem
            // 
            this.lblSelectedItem.AutoSize = true;
            this.lblSelectedItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelectedItem.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSelectedItem.Location = new System.Drawing.Point(10, 10);
            this.lblSelectedItem.Name = "lblSelectedItem";
            this.lblSelectedItem.Size = new System.Drawing.Size(361, 15);
            this.lblSelectedItem.TabIndex = 8;
            this.lblSelectedItem.Text = "Выберите запись в таблице для просмотра деталей";
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.Lavender;
            this.panelFilters.Controls.Add(this.lblEnvironment);
            this.panelFilters.Controls.Add(this.lblSystem);
            this.panelFilters.Controls.Add(this.lblFilter);
            this.panelFilters.Controls.Add(this.txtFilter);
            this.panelFilters.Controls.Add(this.cmbSystemFilter);
            this.panelFilters.Controls.Add(this.cmbEnvironmentFilter);
            this.panelFilters.Controls.Add(this.btnSearch);
            this.panelFilters.Controls.Add(this.btnAddIP);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(10);
            this.panelFilters.Size = new System.Drawing.Size(1000, 60);
            this.panelFilters.TabIndex = 9;
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(488, 15);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(42, 13);
            this.lblEnvironment.TabIndex = 8;
            this.lblEnvironment.Text = "Контур";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(220, 15);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(22, 13);
            this.lblSystem.TabIndex = 7;
            this.lblSystem.Text = "ИС";
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(10, 15);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(17, 13);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "IP";
            // 
            // panelTechInfo
            // 
            this.panelTechInfo.BackColor = System.Drawing.Color.AliceBlue;
            this.panelTechInfo.Controls.Add(this.lblSelectedItem);
            this.panelTechInfo.Controls.Add(this.txtTechInfo);
            this.panelTechInfo.Controls.Add(this.btnRegisterNamen);
            this.panelTechInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTechInfo.Location = new System.Drawing.Point(0, 470);
            this.panelTechInfo.Name = "panelTechInfo";
            this.panelTechInfo.Padding = new System.Windows.Forms.Padding(10);
            this.panelTechInfo.Size = new System.Drawing.Size(1000, 180);
            this.panelTechInfo.TabIndex = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.dgvIPAddresses);
            this.Controls.Add(this.panelTechInfo);
            this.Controls.Add(this.panelFilters);
            this.Name = "MainForm";
            this.Text = "Учет IP-адресов ЕИП";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIPAddresses)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelTechInfo.ResumeLayout(false);
            this.panelTechInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIPAddresses;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.ComboBox cmbSystemFilter;
        private System.Windows.Forms.ComboBox cmbEnvironmentFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAddIP;
        private System.Windows.Forms.Button btnRegisterNamen;
        private System.Windows.Forms.TextBox txtTechInfo;
        private System.Windows.Forms.Label lblSelectedItem;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Panel panelTechInfo;
    }
}