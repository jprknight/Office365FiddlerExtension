namespace EXOFiddlerInspector
{
    partial class Office365AuthUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SAMLResponseParserGroupbox = new System.Windows.Forms.GroupBox();
            this.SaveSigningCertificateButton = new System.Windows.Forms.Button();
            this.OpenSigningCertificateButton = new System.Windows.Forms.Button();
            this.AttributeNameImmutableIDTextBox = new System.Windows.Forms.TextBox();
            this.NameIdentifierFormatTextBox = new System.Windows.Forms.TextBox();
            this.SaveSAMLDataButton = new System.Windows.Forms.Button();
            this.AttributeNameUPNTextBox = new System.Windows.Forms.TextBox();
            this.OpenSAMLDataButton = new System.Windows.Forms.Button();
            this.IssuerTextBox = new System.Windows.Forms.TextBox();
            this.Office365AuthenticationGroupbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AuthenticationResponseCommentsTextbox = new System.Windows.Forms.TextBox();
            this.SigningCertificateGroupbox = new System.Windows.Forms.GroupBox();
            this.SigningCertificateTextbox = new System.Windows.Forms.TextBox();
            this.SAMLResponseParserGroupbox.SuspendLayout();
            this.Office365AuthenticationGroupbox.SuspendLayout();
            this.SigningCertificateGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SAMLResponseParserGroupbox
            // 
            this.SAMLResponseParserGroupbox.Controls.Add(this.SaveSigningCertificateButton);
            this.SAMLResponseParserGroupbox.Controls.Add(this.OpenSigningCertificateButton);
            this.SAMLResponseParserGroupbox.Controls.Add(this.AttributeNameImmutableIDTextBox);
            this.SAMLResponseParserGroupbox.Controls.Add(this.NameIdentifierFormatTextBox);
            this.SAMLResponseParserGroupbox.Controls.Add(this.SaveSAMLDataButton);
            this.SAMLResponseParserGroupbox.Controls.Add(this.AttributeNameUPNTextBox);
            this.SAMLResponseParserGroupbox.Controls.Add(this.OpenSAMLDataButton);
            this.SAMLResponseParserGroupbox.Controls.Add(this.IssuerTextBox);
            this.SAMLResponseParserGroupbox.Location = new System.Drawing.Point(3, 244);
            this.SAMLResponseParserGroupbox.Name = "SAMLResponseParserGroupbox";
            this.SAMLResponseParserGroupbox.Size = new System.Drawing.Size(424, 323);
            this.SAMLResponseParserGroupbox.TabIndex = 0;
            this.SAMLResponseParserGroupbox.TabStop = false;
            this.SAMLResponseParserGroupbox.Text = "SAML Response Parser";
            this.SAMLResponseParserGroupbox.Visible = false;
            this.SAMLResponseParserGroupbox.VisibleChanged += new System.EventHandler(this.SAMLResponseParserGroupbox_VisibleChanged);
            // 
            // SaveSigningCertificateButton
            // 
            this.SaveSigningCertificateButton.Location = new System.Drawing.Point(125, 292);
            this.SaveSigningCertificateButton.Name = "SaveSigningCertificateButton";
            this.SaveSigningCertificateButton.Size = new System.Drawing.Size(143, 23);
            this.SaveSigningCertificateButton.TabIndex = 8;
            this.SaveSigningCertificateButton.Text = "Save Signing Certificate";
            this.SaveSigningCertificateButton.UseVisualStyleBackColor = true;
            this.SaveSigningCertificateButton.Click += new System.EventHandler(this.SaveSigningCertificateButton_Click);
            // 
            // OpenSigningCertificateButton
            // 
            this.OpenSigningCertificateButton.Location = new System.Drawing.Point(125, 263);
            this.OpenSigningCertificateButton.Name = "OpenSigningCertificateButton";
            this.OpenSigningCertificateButton.Size = new System.Drawing.Size(143, 23);
            this.OpenSigningCertificateButton.TabIndex = 3;
            this.OpenSigningCertificateButton.Text = "Open Signing Certificate";
            this.OpenSigningCertificateButton.UseVisualStyleBackColor = true;
            this.OpenSigningCertificateButton.Click += new System.EventHandler(this.OpenSigningCertificateButton_Click);
            // 
            // AttributeNameImmutableIDTextBox
            // 
            this.AttributeNameImmutableIDTextBox.BackColor = System.Drawing.Color.White;
            this.AttributeNameImmutableIDTextBox.Location = new System.Drawing.Point(6, 177);
            this.AttributeNameImmutableIDTextBox.Multiline = true;
            this.AttributeNameImmutableIDTextBox.Name = "AttributeNameImmutableIDTextBox";
            this.AttributeNameImmutableIDTextBox.ReadOnly = true;
            this.AttributeNameImmutableIDTextBox.Size = new System.Drawing.Size(412, 80);
            this.AttributeNameImmutableIDTextBox.TabIndex = 5;
            // 
            // NameIdentifierFormatTextBox
            // 
            this.NameIdentifierFormatTextBox.BackColor = System.Drawing.Color.White;
            this.NameIdentifierFormatTextBox.Location = new System.Drawing.Point(6, 111);
            this.NameIdentifierFormatTextBox.Multiline = true;
            this.NameIdentifierFormatTextBox.Name = "NameIdentifierFormatTextBox";
            this.NameIdentifierFormatTextBox.ReadOnly = true;
            this.NameIdentifierFormatTextBox.Size = new System.Drawing.Size(412, 60);
            this.NameIdentifierFormatTextBox.TabIndex = 4;
            // 
            // SaveSAMLDataButton
            // 
            this.SaveSAMLDataButton.Location = new System.Drawing.Point(274, 292);
            this.SaveSAMLDataButton.Name = "SaveSAMLDataButton";
            this.SaveSAMLDataButton.Size = new System.Drawing.Size(143, 23);
            this.SaveSAMLDataButton.TabIndex = 6;
            this.SaveSAMLDataButton.Text = "Save SAML Data";
            this.SaveSAMLDataButton.UseVisualStyleBackColor = true;
            this.SaveSAMLDataButton.Click += new System.EventHandler(this.SaveSAMLDataButton_Click);
            // 
            // AttributeNameUPNTextBox
            // 
            this.AttributeNameUPNTextBox.BackColor = System.Drawing.Color.White;
            this.AttributeNameUPNTextBox.Location = new System.Drawing.Point(6, 45);
            this.AttributeNameUPNTextBox.Multiline = true;
            this.AttributeNameUPNTextBox.Name = "AttributeNameUPNTextBox";
            this.AttributeNameUPNTextBox.ReadOnly = true;
            this.AttributeNameUPNTextBox.Size = new System.Drawing.Size(412, 60);
            this.AttributeNameUPNTextBox.TabIndex = 2;
            // 
            // OpenSAMLDataButton
            // 
            this.OpenSAMLDataButton.Location = new System.Drawing.Point(274, 263);
            this.OpenSAMLDataButton.Name = "OpenSAMLDataButton";
            this.OpenSAMLDataButton.Size = new System.Drawing.Size(143, 23);
            this.OpenSAMLDataButton.TabIndex = 7;
            this.OpenSAMLDataButton.Text = "Open SAML Data";
            this.OpenSAMLDataButton.UseVisualStyleBackColor = true;
            this.OpenSAMLDataButton.Click += new System.EventHandler(this.OpenSAMLDataButton_Click);
            // 
            // IssuerTextBox
            // 
            this.IssuerTextBox.BackColor = System.Drawing.Color.White;
            this.IssuerTextBox.Location = new System.Drawing.Point(6, 19);
            this.IssuerTextBox.Name = "IssuerTextBox";
            this.IssuerTextBox.ReadOnly = true;
            this.IssuerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.IssuerTextBox.Size = new System.Drawing.Size(412, 20);
            this.IssuerTextBox.TabIndex = 0;
            // 
            // Office365AuthenticationGroupbox
            // 
            this.Office365AuthenticationGroupbox.Controls.Add(this.label1);
            this.Office365AuthenticationGroupbox.Controls.Add(this.AuthenticationResponseCommentsTextbox);
            this.Office365AuthenticationGroupbox.Location = new System.Drawing.Point(3, 3);
            this.Office365AuthenticationGroupbox.Name = "Office365AuthenticationGroupbox";
            this.Office365AuthenticationGroupbox.Size = new System.Drawing.Size(424, 207);
            this.Office365AuthenticationGroupbox.TabIndex = 1;
            this.Office365AuthenticationGroupbox.TabStop = false;
            this.Office365AuthenticationGroupbox.Text = "Office365 Authentication";
            this.Office365AuthenticationGroupbox.VisibleChanged += new System.EventHandler(this.Office365AuthenticationGroupbox_VisibleChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 171);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "To see the SAML response parser find the session with \'SAML Request/Response\' in " +
    "\r\nthe Authentication column.";
            // 
            // AuthenticationResponseCommentsTextbox
            // 
            this.AuthenticationResponseCommentsTextbox.BackColor = System.Drawing.Color.White;
            this.AuthenticationResponseCommentsTextbox.Location = new System.Drawing.Point(9, 19);
            this.AuthenticationResponseCommentsTextbox.Multiline = true;
            this.AuthenticationResponseCommentsTextbox.Name = "AuthenticationResponseCommentsTextbox";
            this.AuthenticationResponseCommentsTextbox.ReadOnly = true;
            this.AuthenticationResponseCommentsTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AuthenticationResponseCommentsTextbox.Size = new System.Drawing.Size(409, 150);
            this.AuthenticationResponseCommentsTextbox.TabIndex = 2;
            // 
            // SigningCertificateGroupbox
            // 
            this.SigningCertificateGroupbox.Controls.Add(this.SigningCertificateTextbox);
            this.SigningCertificateGroupbox.Location = new System.Drawing.Point(433, 3);
            this.SigningCertificateGroupbox.Name = "SigningCertificateGroupbox";
            this.SigningCertificateGroupbox.Size = new System.Drawing.Size(424, 248);
            this.SigningCertificateGroupbox.TabIndex = 2;
            this.SigningCertificateGroupbox.TabStop = false;
            this.SigningCertificateGroupbox.Text = "Signing Certificate";
            this.SigningCertificateGroupbox.Visible = false;
            // 
            // SigningCertificateTextbox
            // 
            this.SigningCertificateTextbox.Location = new System.Drawing.Point(6, 19);
            this.SigningCertificateTextbox.Multiline = true;
            this.SigningCertificateTextbox.Name = "SigningCertificateTextbox";
            this.SigningCertificateTextbox.Size = new System.Drawing.Size(412, 218);
            this.SigningCertificateTextbox.TabIndex = 0;
            // 
            // Office365AuthUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SigningCertificateGroupbox);
            this.Controls.Add(this.SAMLResponseParserGroupbox);
            this.Controls.Add(this.Office365AuthenticationGroupbox);
            this.Name = "Office365AuthUserControl";
            this.Size = new System.Drawing.Size(869, 577);
            this.SAMLResponseParserGroupbox.ResumeLayout(false);
            this.SAMLResponseParserGroupbox.PerformLayout();
            this.Office365AuthenticationGroupbox.ResumeLayout(false);
            this.Office365AuthenticationGroupbox.PerformLayout();
            this.SigningCertificateGroupbox.ResumeLayout(false);
            this.SigningCertificateGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SAMLResponseParserGroupbox;
        private System.Windows.Forms.TextBox AttributeNameImmutableIDTextBox;
        private System.Windows.Forms.TextBox NameIdentifierFormatTextBox;
        private System.Windows.Forms.TextBox AttributeNameUPNTextBox;
        private System.Windows.Forms.TextBox IssuerTextBox;
        private System.Windows.Forms.GroupBox Office365AuthenticationGroupbox;
        private System.Windows.Forms.TextBox AuthenticationResponseCommentsTextbox;
        private System.Windows.Forms.Button SaveSAMLDataButton;
        private System.Windows.Forms.Button OpenSAMLDataButton;
        private System.Windows.Forms.GroupBox SigningCertificateGroupbox;
        private System.Windows.Forms.TextBox SigningCertificateTextbox;
        private System.Windows.Forms.Button OpenSigningCertificateButton;
        private System.Windows.Forms.Button SaveSigningCertificateButton;
        private System.Windows.Forms.Label label1;
    }
}
