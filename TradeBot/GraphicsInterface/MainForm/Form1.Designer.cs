namespace MainForm
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start_trade = new System.Windows.Forms.Button();
            this.comboBox_Algorithms = new System.Windows.Forms.ComboBox();
            this.textBox_ticker = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView_tradeLists = new System.Windows.Forms.DataGridView();
            this.textBox_stock_num = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchased_at = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.breakeven = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lpet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stop_loss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fetched = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exit = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tradeLists)).BeginInit();
            this.SuspendLayout();
            // 
            // button_start_trade
            // 
            this.button_start_trade.Location = new System.Drawing.Point(781, 40);
            this.button_start_trade.Name = "button_start_trade";
            this.button_start_trade.Size = new System.Drawing.Size(87, 35);
            this.button_start_trade.TabIndex = 0;
            this.button_start_trade.Text = "Launch";
            this.button_start_trade.UseVisualStyleBackColor = true;
            this.button_start_trade.Click += new System.EventHandler(this.button_start_trade_Click);
            // 
            // comboBox_Algorithms
            // 
            this.comboBox_Algorithms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Algorithms.FormattingEnabled = true;
            this.comboBox_Algorithms.Items.AddRange(new object[] {
            "Least Profit Exit",
            "Greedy Peek",
            "Standard Deviation"});
            this.comboBox_Algorithms.Location = new System.Drawing.Point(232, 50);
            this.comboBox_Algorithms.Name = "comboBox_Algorithms";
            this.comboBox_Algorithms.Size = new System.Drawing.Size(173, 28);
            this.comboBox_Algorithms.TabIndex = 1;
            // 
            // textBox_ticker
            // 
            this.textBox_ticker.Location = new System.Drawing.Point(74, 50);
            this.textBox_ticker.Name = "textBox_ticker";
            this.textBox_ticker.Size = new System.Drawing.Size(136, 26);
            this.textBox_ticker.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ticker";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dataGridView_tradeLists
            // 
            this.dataGridView_tradeLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_tradeLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Ticker,
            this.purchased_at,
            this.breakeven,
            this.lpet,
            this.target,
            this.stop_loss,
            this.fetched,
            this.Exit});
            this.dataGridView_tradeLists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_tradeLists.Location = new System.Drawing.Point(18, 99);
            this.dataGridView_tradeLists.Name = "dataGridView_tradeLists";
            this.dataGridView_tradeLists.RowTemplate.Height = 28;
            this.dataGridView_tradeLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_tradeLists.Size = new System.Drawing.Size(1238, 273);
            this.dataGridView_tradeLists.TabIndex = 5;
            this.dataGridView_tradeLists.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_tradeLists_CellContentClick);
            // 
            // textBox_stock_num
            // 
            this.textBox_stock_num.Location = new System.Drawing.Point(554, 50);
            this.textBox_stock_num.Name = "textBox_stock_num";
            this.textBox_stock_num.Size = new System.Drawing.Size(99, 26);
            this.textBox_stock_num.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(489, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Stock#";
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            // 
            // Ticker
            // 
            this.Ticker.HeaderText = "Name";
            this.Ticker.Name = "Ticker";
            this.Ticker.ReadOnly = true;
            // 
            // purchased_at
            // 
            this.purchased_at.HeaderText = "Bought price";
            this.purchased_at.Name = "purchased_at";
            this.purchased_at.ReadOnly = true;
            // 
            // breakeven
            // 
            this.breakeven.HeaderText = "Break Even";
            this.breakeven.Name = "breakeven";
            this.breakeven.ReadOnly = true;
            // 
            // lpet
            // 
            this.lpet.HeaderText = "LPET";
            this.lpet.Name = "lpet";
            this.lpet.ReadOnly = true;
            // 
            // target
            // 
            this.target.HeaderText = "Target Price";
            this.target.Name = "target";
            this.target.ReadOnly = true;
            // 
            // stop_loss
            // 
            this.stop_loss.HeaderText = "Stop Loss";
            this.stop_loss.Name = "stop_loss";
            this.stop_loss.ReadOnly = true;
            // 
            // fetched
            // 
            this.fetched.HeaderText = "Current Price";
            this.fetched.Name = "fetched";
            this.fetched.ReadOnly = true;
            // 
            // Exit
            // 
            this.Exit.HeaderText = "Liquidate";
            this.Exit.Name = "Exit";
            this.Exit.ReadOnly = true;
            this.Exit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Exit.Text = "SELL";
            this.Exit.ToolTipText = "sell ";
            this.Exit.UseColumnTextForButtonValue = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 602);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_stock_num);
            this.Controls.Add(this.dataGridView_tradeLists);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_ticker);
            this.Controls.Add(this.comboBox_Algorithms);
            this.Controls.Add(this.button_start_trade);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tradeLists)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start_trade;
        private System.Windows.Forms.ComboBox comboBox_Algorithms;
        private System.Windows.Forms.TextBox textBox_ticker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView_tradeLists;
        private System.Windows.Forms.TextBox textBox_stock_num;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchased_at;
        private System.Windows.Forms.DataGridViewTextBoxColumn breakeven;
        private System.Windows.Forms.DataGridViewTextBoxColumn lpet;
        private System.Windows.Forms.DataGridViewTextBoxColumn target;
        private System.Windows.Forms.DataGridViewTextBoxColumn stop_loss;
        private System.Windows.Forms.DataGridViewTextBoxColumn fetched;
        private System.Windows.Forms.DataGridViewButtonColumn Exit;
    }
}

