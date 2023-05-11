﻿using HCMIS.Components;
using HCMIS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCMIS
{
    public partial class PatientProfileForm : Form
    {
        #region Resize

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int resizeableThickness = 3;

        new Rectangle Left { get { return new Rectangle(0, 0, resizeableThickness, this.ClientSize.Height); } }
        new Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - resizeableThickness, this.ClientSize.Width, resizeableThickness); } }
        new Rectangle Right { get { return new Rectangle(this.ClientSize.Width - resizeableThickness, 0, resizeableThickness, this.ClientSize.Height); } }
        Rectangle BottomLeft { get { return new Rectangle(0, ClientSize.Height - resizeableThickness, resizeableThickness, resizeableThickness); } }
        Rectangle BottomRight { get { return new Rectangle(ClientSize.Width - resizeableThickness, ClientSize.Height - resizeableThickness, resizeableThickness, resizeableThickness); } }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84)
            {
                Point cursor = PointToClient(Cursor.Position);

                if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }
        #endregion

        #region Draggable
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessge(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void MouseDrag(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessge(Handle, 0x112, 0xf012, 0);
        }
        #endregion

        public PatientProfileForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            // Set Maximized Window Size
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
        }

        public PatientProfileForm(Patient patient)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            // Set Maximized Window Size
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;

            nameLabel.Text += patient.Fullname;
            idLabel.Text += patient.ID.ToString();
            genderLabel.Text += patient.Gender;
            birthdayLabel.Text += patient.Birthday.ToString("MMMM dd, yyyy");
            addressLabel.Text += patient.Address;
            emailLabel.Text += patient.Contact.Email;
            phoneNumberLabel.Text += patient.Contact.PhoneNumber;
            bloodtypeLabel.Text += patient.Bloodtype;
            maritalStatus.Text += patient.MaritalStatus;
            numberOfKidsLabel.Text += patient.NumberOfKids.ToString();
            totalVisitLabel.Text += patient.TotalVisit.ToString();

            List<Record> records = patient.GetRecords();
            records.ForEach(record =>
            {
                tableGrid.Rows.Add(record.ID, record.VisitDateTime.ToString("MMMM dd, yyyy"), record.VisitDateTime.ToString("hh:mm tt"), record.AssignedWorker, record.Reason, record.Remarks, record.Deferred ? "Yes" : "No");
            });
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}
