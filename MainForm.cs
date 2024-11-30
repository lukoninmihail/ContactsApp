using ContactsApp.Models;
using ContactsApp.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ContactsApp
{
    public partial class MainForm : Form
    {
        private Project _project = null!;
        private ListBox _contactsListBox = null!;
        private TextBox _searchTextBox = null!;
        private TableLayoutPanel _detailsTable = null!;
        private Label _birthdayLabel = null!;

        public MainForm()
        {
            LoadContacts();
            SetupUI();
            RefreshContactsList();
            DisplayBirthdays();
        }

        private void SetupUI()
        {
            this.Text = "ContactsApp";
            this.Size = new Size(900, 600);
            this.MinimumSize = new Size(700, 500);

            var menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Exit", null, CloseApp);
            var editMenu = new ToolStripMenuItem("Edit");
            editMenu.DropDownItems.Add("Add Contact", null, AddContact);
            editMenu.DropDownItems.Add("Edit Contact", null, EditContact);
            editMenu.DropDownItems.Add("Remove Contact", null, RemoveContact);
            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("About", null, ShowAbout);
            menuStrip.Items.AddRange(new[] { fileMenu, editMenu, helpMenu });
            this.MainMenuStrip = menuStrip;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            mainLayout.Controls.Add(menuStrip, 0, 0);

            var contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.Controls.Add(contentLayout, 1, 1);

            SetupLeftPanel(contentLayout);
            SetupRightPanel(contentLayout);

            this.Controls.Add(mainLayout);
        }

        private void SetupLeftPanel(TableLayoutPanel contentLayout)
        {
            var leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            var findPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2
            };
            findPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            findPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var findLabel = new Label
            {
                Text = "Find:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };
            _searchTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                PlaceholderText = "Enter surname..."
            };
            _searchTextBox.TextChanged += SearchContacts;
            findPanel.Controls.Add(findLabel, 0, 0);
            findPanel.Controls.Add(_searchTextBox, 1, 0);

            _contactsListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                DisplayMember = "Surname"
            };
            _contactsListBox.SelectedIndexChanged += OnContactSelected;

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };

            buttonsPanel.Controls.Add(CreateImageButton(Properties.Resources.add_icon, "Add", AddContact));
            buttonsPanel.Controls.Add(CreateImageButton(Properties.Resources.edit_icon, "Edit", EditContact));
            buttonsPanel.Controls.Add(CreateImageButton(Properties.Resources.remove_icon, "Remove", RemoveContact));

            leftPanel.Controls.Add(findPanel, 0, 0);
            leftPanel.Controls.Add(_contactsListBox, 1, 1);
            leftPanel.Controls.Add(buttonsPanel, 2, 2);

            contentLayout.Controls.Add(leftPanel, 0, 0);
        }

        private void SetupRightPanel(TableLayoutPanel contentLayout)
        {
            var rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            _detailsTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2
            };
            _detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            AddDetailRow("Surname:", 0);
            AddDetailRow("Name:", 1);
            AddDetailRow("Birthday:", 2);
            AddDetailRow("Phone:", 3);
            AddDetailRow("E-mail:", 4);
            AddDetailRow("vk.com:", 5);

            _birthdayLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Fill,
                BackColor = Color.LightYellow,
                Padding = new Padding(5),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            rightPanel.Controls.Add(_detailsTable, 0, 0);
            rightPanel.Controls.Add(_birthdayLabel, 1, 1);

            contentLayout.Controls.Add(rightPanel, 1, 0);
        }

        private void AddDetailRow(string labelText, int rowIndex)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var valueBox = new TextBox
            {
                ReadOnly = true,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            _detailsTable.Controls.Add(label, 0, rowIndex);
            _detailsTable.Controls.Add(valueBox, 1, rowIndex);
        }

        private Button CreateImageButton(Bitmap image, string toolTip, EventHandler onClick)
        {
            var button = new Button
            {
                Image = new Bitmap(image, new Size(32, 32)),
                Size = new Size(50, 50),
                FlatStyle = FlatStyle.Flat
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += onClick;
            var toolTipControl = new ToolTip();
            toolTipControl.SetToolTip(button, toolTip);
            return button;
        }

        private void SearchContacts(object? sender, EventArgs e)
        {
            RefreshContactsList(_searchTextBox.Text);
        }

        private void OnContactSelected(object? sender, EventArgs e)
        {
            if (_contactsListBox.SelectedItem is Contact selectedContact)
            {
                DisplayContactDetails(selectedContact);
            }
            else
            {
                ClearContactDetails();
            }
        }

        private void DisplayContactDetails(Contact contact)
        {
            SetDetailValue(0, contact.Surname);
            SetDetailValue(1, contact.Name);
            SetDetailValue(2, contact.BirthDate.ToShortDateString());
            SetDetailValue(3, contact.PhoneNumber.Number);
            SetDetailValue(4, contact.Email);
            SetDetailValue(5, contact.VKId);
        }

        private void ClearContactDetails()
        {
            for (int i = 0; i < 6; i++)
            {
                SetDetailValue(i, "");
            }
        }

        private void SetDetailValue(int rowIndex, string value)
        {
            var valueBox = _detailsTable.GetControlFromPosition(1, rowIndex) as TextBox;
            if (valueBox != null)
            {
                valueBox.Text = value;
            }
        }

        private void AddContact(object? sender, EventArgs e)
        {
            var form = new EditContactForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _project.Contacts.Add(form.Contact);
                ProjectManager.Save(_project);
                RefreshContactsList();
                DisplayBirthdays(); 
            }
        }

        private void EditContact(object? sender, EventArgs e)
        {
            if (_contactsListBox.SelectedItem is not Contact selectedContact) return;

            var form = new EditContactForm(selectedContact);
            if (form.ShowDialog() == DialogResult.OK)
            {
                int index = _project.Contacts.IndexOf(selectedContact);
                _project.Contacts[index] = form.Contact;
                ProjectManager.Save(_project);
                RefreshContactsList();
                DisplayBirthdays(); 
            }
        }

        private void RemoveContact(object? sender, EventArgs e)
        {
            if (_contactsListBox.SelectedItem is not Contact selectedContact) return;

            var result = MessageBox.Show($"Are you sure you want to delete {selectedContact.Surname}?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _project.Contacts.Remove(selectedContact);
                ProjectManager.Save(_project);
                RefreshContactsList();
                DisplayBirthdays(); 
            }
        }

        private void RefreshContactsList(string filter = "")
        {
            _contactsListBox.Items.Clear();
            var filteredContacts = string.IsNullOrWhiteSpace(filter)
                ? _project.GetSortedContacts()
                : _project.GetContactsBySubstring(filter);

            _contactsListBox.Items.AddRange(filteredContacts.ToArray());

            if (_contactsListBox.Items.Count > 0)
            {
                _contactsListBox.SelectedIndex = 0;
            }
            else
            {
                ClearContactDetails();
            }
        }

        private void DisplayBirthdays()
        {
            var today = DateTime.Today;
            var birthdayContacts = _project.Contacts
                .Where(c => c.BirthDate.Month == today.Month && c.BirthDate.Day == today.Day)
                .ToList();

            _birthdayLabel.Text = birthdayContacts.Any()
                ? $"Сегодня день рождения у: {string.Join(", ", birthdayContacts.Select(c => $"{c.Surname} {c.Name}"))}"
                : "Сегодня нет дней рождения.";
        }

        private void LoadContacts()
        {
            _project = ProjectManager.Load();
        }

        private void ShowAbout(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "ContactsApp v. 1.0.0\n\nAuthor: student\n" +
                "e-mail for feedback: student@mail.com\n" +
                "GitHub: student/ContactsApp\n\n2024 Student ©",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CloseApp(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
