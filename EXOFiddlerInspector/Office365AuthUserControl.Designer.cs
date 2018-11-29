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
            this.SAMLResponseParser = new System.Windows.Forms.GroupBox();
            this.AttributeNameImmutableIDTextBox = new System.Windows.Forms.TextBox();
            this.NameIdentifierFormatTextBox = new System.Windows.Forms.TextBox();
            this.AttributeNameUPNTextBox = new System.Windows.Forms.TextBox();
            this.IssuerTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AuthenticationResponseComments = new System.Windows.Forms.TextBox();
            this.SaveSAMLDataButton = new System.Windows.Forms.Button();
            this.OpenSAMLDataButton = new System.Windows.Forms.Button();
            this.SAMLResponseParser.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SAMLResponseParser
            // 
            this.SAMLResponseParser.Controls.Add(this.OpenSAMLDataButton);
            this.SAMLResponseParser.Controls.Add(this.SaveSAMLDataButton);
            this.SAMLResponseParser.Controls.Add(this.AttributeNameImmutableIDTextBox);
            this.SAMLResponseParser.Controls.Add(this.NameIdentifierFormatTextBox);
            this.SAMLResponseParser.Controls.Add(this.AttributeNameUPNTextBox);
            this.SAMLResponseParser.Controls.Add(this.IssuerTextBox);
            this.SAMLResponseParser.Location = new System.Drawing.Point(3, 178);
            this.SAMLResponseParser.Name = "SAMLResponseParser";
            this.SAMLResponseParser.Size = new System.Drawing.Size(424, 275);
            this.SAMLResponseParser.TabIndex = 0;
            this.SAMLResponseParser.TabStop = false;
            this.SAMLResponseParser.Text = "SAML Response Parser";
            // 
            // AttributeNameImmutableIDTextBox
            // 
            this.AttributeNameImmutableIDTextBox.BackColor = System.Drawing.Color.White;
            this.AttributeNameImmutableIDTextBox.Location = new System.Drawing.Point(6, 177);
            this.AttributeNameImmutableIDTextBox.Multiline = true;
            this.AttributeNameImmutableIDTextBox.Name = "AttributeNameImmutableIDTextBox";
            this.AttributeNameImmutableIDTextBox.ReadOnly = true;
            this.AttributeNameImmutableIDTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AttributeNameImmutableIDTextBox.Size = new System.Drawing.Size(412, 60);
            this.AttributeNameImmutableIDTextBox.TabIndex = 5;
            // 
            // NameIdentifierFormatTextBox
            // 
            this.NameIdentifierFormatTextBox.BackColor = System.Drawing.Color.White;
            this.NameIdentifierFormatTextBox.Location = new System.Drawing.Point(6, 111);
            this.NameIdentifierFormatTextBox.Multiline = true;
            this.NameIdentifierFormatTextBox.Name = "NameIdentifierFormatTextBox";
            this.NameIdentifierFormatTextBox.ReadOnly = true;
            this.NameIdentifierFormatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.NameIdentifierFormatTextBox.Size = new System.Drawing.Size(412, 60);
            this.NameIdentifierFormatTextBox.TabIndex = 4;
            // 
            // AttributeNameUPNTextBox
            // 
            this.AttributeNameUPNTextBox.BackColor = System.Drawing.Color.White;
            this.AttributeNameUPNTextBox.Location = new System.Drawing.Point(6, 45);
            this.AttributeNameUPNTextBox.Multiline = true;
            this.AttributeNameUPNTextBox.Name = "AttributeNameUPNTextBox";
            this.AttributeNameUPNTextBox.ReadOnly = true;
            this.AttributeNameUPNTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AttributeNameUPNTextBox.Size = new System.Drawing.Size(412, 60);
            this.AttributeNameUPNTextBox.TabIndex = 2;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AuthenticationResponseComments);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 169);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Office365 Authentication";
            // 
            // AuthenticationResponseComments
            // 
            this.AuthenticationResponseComments.BackColor = System.Drawing.Color.White;
            this.AuthenticationResponseComments.Location = new System.Drawing.Point(9, 19);
            this.AuthenticationResponseComments.Multiline = true;
            this.AuthenticationResponseComments.Name = "AuthenticationResponseComments";
            this.AuthenticationResponseComments.ReadOnly = true;
            this.AuthenticationResponseComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AuthenticationResponseComments.Size = new System.Drawing.Size(409, 140);
            this.AuthenticationResponseComments.TabIndex = 2;
            // 
            // SaveSAMLDataButton
            // 
            this.SaveSAMLDataButton.Location = new System.Drawing.Point(296, 243);
            this.SaveSAMLDataButton.Name = "SaveSAMLDataButton";
            this.SaveSAMLDataButton.Size = new System.Drawing.Size(122, 23);
            this.SaveSAMLDataButton.TabIndex = 6;
            this.SaveSAMLDataButton.Text = "Save SAML Data";
            this.SaveSAMLDataButton.UseVisualStyleBackColor = true;
            this.SaveSAMLDataButton.Click += new System.EventHandler(this.SaveSAMLDataButton_Click);
            // 
            // OpenSAMLDataButton
            // 
            this.OpenSAMLDataButton.Location = new System.Drawing.Point(170, 243);
            this.OpenSAMLDataButton.Name = "OpenSAMLDataButton";
            this.OpenSAMLDataButton.Size = new System.Drawing.Size(120, 23);
            this.OpenSAMLDataButton.TabIndex = 7;
            this.OpenSAMLDataButton.Text = "Open SAML Data";
            this.OpenSAMLDataButton.UseVisualStyleBackColor = true;
            this.OpenSAMLDataButton.Click += new System.EventHandler(this.OpenSAMLDataButton_Click);
            // 
            // Office365AuthUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SAMLResponseParser);
            this.Name = "Office365AuthUserControl";
            this.Size = new System.Drawing.Size(441, 465);
            this.SAMLResponseParser.ResumeLayout(false);
            this.SAMLResponseParser.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SAMLResponseParser;
        private System.Windows.Forms.TextBox AttributeNameImmutableIDTextBox;
        private System.Windows.Forms.TextBox NameIdentifierFormatTextBox;
        private System.Windows.Forms.TextBox AttributeNameUPNTextBox;
        private System.Windows.Forms.TextBox IssuerTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox AuthenticationResponseComments;
        private System.Windows.Forms.Button SaveSAMLDataButton;
        private System.Windows.Forms.Button OpenSAMLDataButton;
    }
}
