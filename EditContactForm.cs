using System;
using System.Drawing;
using System.Windows.Forms;
using ContactsApp.Models;
using ContactsApp.Utils;

namespace ContactsApp
{
    public partial class EditContactForm : Form
    {
        private TextBox _surnameTextBox = null!;
        private TextBox _nameTextBox = null!;
        private TextBox _phoneTextBox = null!;
        private DateTimePicker _birthDatePicker = null!;
        private TextBox _emailTextBox = null!;
        private TextBox _vkIdTextBox = null!;
        private Button _okButton = null!;
        private Button _cancelButton = null!;

        public Contact Contact { get; private set; }

        public EditContactForm(Contact? contact = null)
        {
            Contact = contact != null ? (Contact)contact.Clone() : new Contact();
            SetupUI();
            FillFields();
        }

        private void SetupUI()
        {
            this.Text = "Add/Edit Contact";
            this.Size = new Size(350, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 7,
                ColumnCount = 2,
                Padding = new Padding(10),
                BackColor = SystemColors.Control
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 7; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            AddFormField(mainPanel, "Surname:", _surnameTextBox = new TextBox(), 0);

            AddFormField(mainPanel, "Name:", _nameTextBox = new TextBox(), 1);

            AddFormField(mainPanel, "Birthday:", _birthDatePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                MaxDate = DateTime.Now,
                MinDate = new DateTime(1900, 1, 1)
            }, 2);

            AddFormField(mainPanel, "Phone:", _phoneTextBox = new TextBox(), 3);

            AddFormField(mainPanel, "E-mail:", _emailTextBox = new TextBox(), 4);

            AddFormField(mainPanel, "vk.com:", _vkIdTextBox = new TextBox(), 5);

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Padding(0, 10, 0, 0)
            };

            _okButton = new Button
            {
                Text = "OK",
                Width = 80,
                Height = 25,
                DialogResult = DialogResult.OK
            };

            _cancelButton = new Button
            {
                Text = "Cancel",
                Width = 80,
                Height = 25,
                DialogResult = DialogResult.Cancel,
                Margin = new Padding(0, 0, 10, 0)
            };

            _okButton.Click += (sender, e) => SaveContact();
            _cancelButton.Click += (sender, e) => this.Close();

            buttonsPanel.Controls.Add(_okButton);
            buttonsPanel.Controls.Add(_cancelButton);

            mainPanel.Controls.Add(buttonsPanel, 0, 6);
            mainPanel.SetColumnSpan(buttonsPanel, 2);

            this.Controls.Add(mainPanel);
        }

        private void AddFormField(TableLayoutPanel panel, string labelText, Control control, int row)
        {
            var label = new Label
            {
                Text = labelText,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 5, 5)
            };

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 5, 0, 5);

            panel.Controls.Add(label, 0, row);
            panel.Controls.Add(control, 1, row);
        }

        private void FillFields()
        {
            _surnameTextBox.Text = Contact.Surname;
            _nameTextBox.Text = Contact.Name;
            _phoneTextBox.Text = Contact.PhoneNumber?.Number ?? "";

            var birthDate = Contact.BirthDate;
            if (birthDate < _birthDatePicker.MinDate || birthDate > _birthDatePicker.MaxDate)
            {
                _birthDatePicker.Value = DateTime.Today;
            }
            else
            {
                _birthDatePicker.Value = birthDate;
            }

            _emailTextBox.Text = Contact.Email;
            _vkIdTextBox.Text = Contact.VKId;
        }

        private void SaveContact()
        {
            if (!ValidationUtils.IsValidName(_surnameTextBox.Text))
            {
                MessageBox.Show("Фамилия должна быть не длиннее 50 символов и не пустой.", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidationUtils.IsValidName(_nameTextBox.Text))
            {
                MessageBox.Show("Имя должно быть не длиннее 50 символов и не пустым.", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var phoneNumber = new PhoneNumber { Number = _phoneTextBox.Text };
            if (!phoneNumber.IsValid())
            {
                MessageBox.Show("Телефон должен быть числом из 11 цифр и начинаться с '7'.", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidationUtils.IsValidEmail(_emailTextBox.Text))
            {
                MessageBox.Show("E-mail должен быть корректным и содержать не более 50 символов.", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidationUtils.IsValidVKId(_vkIdTextBox.Text))
            {
                MessageBox.Show("VK ID должен содержать не более 15 символов.", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Contact.Surname = _surnameTextBox.Text;
            Contact.Name = _nameTextBox.Text;
            Contact.PhoneNumber = phoneNumber;
            Contact.BirthDate = _birthDatePicker.Value;
            Contact.Email = _emailTextBox.Text;
            Contact.VKId = _vkIdTextBox.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
