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
            this.AuthLabel = new System.Windows.Forms.Label();
            this.AuthTextBox = new System.Windows.Forms.TextBox();
            this.SAMLResponseParser.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SAMLResponseParser
            // 
            this.SAMLResponseParser.Controls.Add(this.AttributeNameImmutableIDTextBox);
            this.SAMLResponseParser.Controls.Add(this.NameIdentifierFormatTextBox);
            this.SAMLResponseParser.Controls.Add(this.AttributeNameUPNTextBox);
            this.SAMLResponseParser.Controls.Add(this.IssuerTextBox);
            this.SAMLResponseParser.Location = new System.Drawing.Point(3, 205);
            this.SAMLResponseParser.Name = "SAMLResponseParser";
            this.SAMLResponseParser.Size = new System.Drawing.Size(412, 189);
            this.SAMLResponseParser.TabIndex = 0;
            this.SAMLResponseParser.TabStop = false;
            this.SAMLResponseParser.Text = "SAML Response Parser";
            // 
            // AttributeNameImmutableIDTextBox
            // 
            this.AttributeNameImmutableIDTextBox.Location = new System.Drawing.Point(6, 137);
            this.AttributeNameImmutableIDTextBox.Multiline = true;
            this.AttributeNameImmutableIDTextBox.Name = "AttributeNameImmutableIDTextBox";
            this.AttributeNameImmutableIDTextBox.Size = new System.Drawing.Size(397, 40);
            this.AttributeNameImmutableIDTextBox.TabIndex = 5;
            // 
            // NameIdentifierFormatTextBox
            // 
            this.NameIdentifierFormatTextBox.Location = new System.Drawing.Point(6, 91);
            this.NameIdentifierFormatTextBox.Multiline = true;
            this.NameIdentifierFormatTextBox.Name = "NameIdentifierFormatTextBox";
            this.NameIdentifierFormatTextBox.Size = new System.Drawing.Size(397, 40);
            this.NameIdentifierFormatTextBox.TabIndex = 4;
            // 
            // AttributeNameUPNTextBox
            // 
            this.AttributeNameUPNTextBox.Location = new System.Drawing.Point(6, 45);
            this.AttributeNameUPNTextBox.Multiline = true;
            this.AttributeNameUPNTextBox.Name = "AttributeNameUPNTextBox";
            this.AttributeNameUPNTextBox.Size = new System.Drawing.Size(397, 40);
            this.AttributeNameUPNTextBox.TabIndex = 2;
            // 
            // IssuerTextBox
            // 
            this.IssuerTextBox.Location = new System.Drawing.Point(6, 19);
            this.IssuerTextBox.Name = "IssuerTextBox";
            this.IssuerTextBox.Size = new System.Drawing.Size(397, 20);
            this.IssuerTextBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AuthenticationResponseComments);
            this.groupBox1.Controls.Add(this.AuthLabel);
            this.groupBox1.Controls.Add(this.AuthTextBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 196);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Office365 Authentication";
            // 
            // AuthenticationResponseComments
            // 
            this.AuthenticationResponseComments.Location = new System.Drawing.Point(9, 45);
            this.AuthenticationResponseComments.Multiline = true;
            this.AuthenticationResponseComments.Name = "AuthenticationResponseComments";
            this.AuthenticationResponseComments.Size = new System.Drawing.Size(394, 140);
            this.AuthenticationResponseComments.TabIndex = 2;
            // 
            // AuthLabel
            // 
            this.AuthLabel.AutoSize = true;
            this.AuthLabel.Location = new System.Drawing.Point(6, 22);
            this.AuthLabel.Name = "AuthLabel";
            this.AuthLabel.Size = new System.Drawing.Size(75, 13);
            this.AuthLabel.TabIndex = 77;
            this.AuthLabel.Text = "Authentication";
            // 
            // AuthTextBox
            // 
            this.AuthTextBox.Location = new System.Drawing.Point(87, 19);
            this.AuthTextBox.Name = "AuthTextBox";
            this.AuthTextBox.Size = new System.Drawing.Size(316, 20);
            this.AuthTextBox.TabIndex = 78;
            // 
            // Office365AuthUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SAMLResponseParser);
            this.Name = "Office365AuthUserControl";
            this.Size = new System.Drawing.Size(425, 407);
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
        private System.Windows.Forms.Label AuthLabel;
        private System.Windows.Forms.TextBox AuthTextBox;
    }
}
