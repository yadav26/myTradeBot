﻿namespace MainForm
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
            this.textBox_stock_num = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView_tradeLists = new System.Windows.Forms.DataGridView();
            this.Exit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchased_at = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.order_placed_at = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tradeLists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_start_trade
            // 
            this.button_start_trade.Location = new System.Drawing.Point(633, 51);
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
            // textBox_stock_num
            // 
            this.textBox_stock_num.Location = new System.Drawing.Point(507, 50);
            this.textBox_stock_num.Name = "textBox_stock_num";
            this.textBox_stock_num.Size = new System.Drawing.Size(99, 26);
            this.textBox_stock_num.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(442, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Stock#";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(18, 107);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView_tradeLists);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1149, 469);
            this.splitContainer1.SplitterDistance = 765;
            this.splitContainer1.TabIndex = 8;
            // 
            // dataGridView_tradeLists
            // 
            this.dataGridView_tradeLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_tradeLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Exit,
            this.Id,
            this.Ticker,
            this.purchased_at,
            this.units,
            this.order_placed_at});
            this.dataGridView_tradeLists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_tradeLists.Location = new System.Drawing.Point(22, 13);
            this.dataGridView_tradeLists.Name = "dataGridView_tradeLists";
            this.dataGridView_tradeLists.RowTemplate.Height = 28;
            this.dataGridView_tradeLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_tradeLists.Size = new System.Drawing.Size(668, 275);
            this.dataGridView_tradeLists.TabIndex = 5;
            this.dataGridView_tradeLists.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_tradeLists_CellContentClick);
            // 
            // Exit
            // 
            this.Exit.HeaderText = "SELL";
            this.Exit.Name = "Exit";
            this.Exit.ReadOnly = true;
            this.Exit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Exit.Text = "SELL";
            this.Exit.ToolTipText = "sell ";
            this.Exit.UseColumnTextForButtonValue = true;
            this.Exit.Width = 50;
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.MinimumWidth = 10;
            this.Id.Name = "Id";
            this.Id.Width = 40;
            // 
            // Ticker
            // 
            this.Ticker.HeaderText = "Name";
            this.Ticker.MaxInputLength = 100;
            this.Ticker.MinimumWidth = 50;
            this.Ticker.Name = "Ticker";
            this.Ticker.ReadOnly = true;
            this.Ticker.Width = 50;
            // 
            // purchased_at
            // 
            this.purchased_at.HeaderText = "Bought price";
            this.purchased_at.MinimumWidth = 75;
            this.purchased_at.Name = "purchased_at";
            this.purchased_at.ReadOnly = true;
            this.purchased_at.Width = 75;
            // 
            // units
            // 
            this.units.HeaderText = "Units";
            this.units.MinimumWidth = 50;
            this.units.Name = "units";
            this.units.Width = 50;
            // 
            // order_placed_at
            // 
            this.order_placed_at.HeaderText = "Date Time of purchase";
            this.order_placed_at.Name = "order_placed_at";
            this.order_placed_at.Width = 150;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 15;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(326, 437);
            this.dataGridView1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 681);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_stock_num);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_ticker);
            this.Controls.Add(this.comboBox_Algorithms);
            this.Controls.Add(this.button_start_trade);
            this.Name = "Form1";
            this.Text = "Day Trade Bot";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tradeLists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start_trade;
        private System.Windows.Forms.ComboBox comboBox_Algorithms;
        private System.Windows.Forms.TextBox textBox_ticker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_stock_num;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_placed_at;
        private System.Windows.Forms.DataGridViewTextBoxColumn units;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchased_at;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewButtonColumn Exit;
        private System.Windows.Forms.DataGridView dataGridView_tradeLists;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

