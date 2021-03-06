﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

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
            this.splitContainer_ActiveOrders = new System.Windows.Forms.SplitContainer();
            this.dataGridView_ActiveOrderList = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.cartesianChart2 = new LiveCharts.Wpf.CartesianChart();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button_MarketAnalyse = new System.Windows.Forms.Button();
            this.groupBox_MarketAnalysis = new System.Windows.Forms.GroupBox();
            this.radioButton_BSE = new System.Windows.Forms.RadioButton();
            this.radioButton_NSE = new System.Windows.Forms.RadioButton();
            this.splitContainer_MarketAnalysis = new System.Windows.Forms.SplitContainer();
            this.dataGridView_MarketAnalysis = new System.Windows.Forms.DataGridView();
            this.ObserveNow = new System.Windows.Forms.DataGridViewButtonColumn();
            this.progressBar_MarketAnalysis = new System.Windows.Forms.ProgressBar();
            this.splitContainer_Scanner = new System.Windows.Forms.SplitContainer();
            this.dataGridView_Scanner = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer_CompletedOrders = new System.Windows.Forms.SplitContainer();
            this.dataGridView_CompletedOrders = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Filter = new System.Windows.Forms.TextBox();
            this.button_ApplyFilter = new System.Windows.Forms.Button();
            this.button_ClearFilter = new System.Windows.Forms.Button();
            this.label_investment = new System.Windows.Forms.Label();
            this.label_profit = new System.Windows.Forms.Label();
            this.OrderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Purchased_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Current_Profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopLoss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exit_Target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BreakEven = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeastProfitExit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pruchase_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_ActiveOrders)).BeginInit();
            this.splitContainer_ActiveOrders.Panel1.SuspendLayout();
            this.splitContainer_ActiveOrders.Panel2.SuspendLayout();
            this.splitContainer_ActiveOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ActiveOrderList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox_MarketAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_MarketAnalysis)).BeginInit();
            this.splitContainer_MarketAnalysis.Panel1.SuspendLayout();
            this.splitContainer_MarketAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_MarketAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Scanner)).BeginInit();
            this.splitContainer_Scanner.Panel1.SuspendLayout();
            this.splitContainer_Scanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Scanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_CompletedOrders)).BeginInit();
            this.splitContainer_CompletedOrders.Panel1.SuspendLayout();
            this.splitContainer_CompletedOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CompletedOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer_ActiveOrders
            // 
            this.splitContainer_ActiveOrders.Location = new System.Drawing.Point(18, 242);
            this.splitContainer_ActiveOrders.Name = "splitContainer_ActiveOrders";
            // 
            // splitContainer_ActiveOrders.Panel1
            // 
            this.splitContainer_ActiveOrders.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer_ActiveOrders.Panel1.Controls.Add(this.dataGridView_ActiveOrderList);
            // 
            // splitContainer_ActiveOrders.Panel2
            // 
            this.splitContainer_ActiveOrders.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer_ActiveOrders.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer_ActiveOrders.Size = new System.Drawing.Size(1003, 257);
            this.splitContainer_ActiveOrders.SplitterDistance = 860;
            this.splitContainer_ActiveOrders.TabIndex = 8;
            // 
            // dataGridView_ActiveOrderList
            // 
            this.dataGridView_ActiveOrderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ActiveOrderList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderID,
            this.Ticker,
            this.Units,
            this.Purchased_Price,
            this.CurrentPrice,
            this.Current_Profit,
            this.StopLoss,
            this.Exit_Target,
            this.BreakEven,
            this.LeastProfitExit,
            this.Pruchase_Time});
            this.dataGridView_ActiveOrderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ActiveOrderList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_ActiveOrderList.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_ActiveOrderList.Name = "dataGridView_ActiveOrderList";
            this.dataGridView_ActiveOrderList.RowTemplate.Height = 28;
            this.dataGridView_ActiveOrderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_ActiveOrderList.Size = new System.Drawing.Size(860, 257);
            this.dataGridView_ActiveOrderList.TabIndex = 5;
            this.dataGridView_ActiveOrderList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_ActiveOrder_CellContentClick);
            this.dataGridView_ActiveOrderList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_ActiveOrderList_CellFormatting);
            this.dataGridView_ActiveOrderList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView_ActiveOrderList_RowsAdded);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 15;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(139, 257);
            this.dataGridView1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(23, 758);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.elementHost1);
            this.splitContainer2.Size = new System.Drawing.Size(1005, 173);
            this.splitContainer2.SplitterDistance = 625;
            this.splitContainer2.TabIndex = 10;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(625, 173);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.cartesianChart2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // button_MarketAnalyse
            // 
            this.button_MarketAnalyse.Location = new System.Drawing.Point(24, 85);
            this.button_MarketAnalyse.Name = "button_MarketAnalyse";
            this.button_MarketAnalyse.Size = new System.Drawing.Size(188, 42);
            this.button_MarketAnalyse.TabIndex = 4;
            this.button_MarketAnalyse.Text = "Analyse Market Trend";
            this.button_MarketAnalyse.UseVisualStyleBackColor = true;
            this.button_MarketAnalyse.Click += new System.EventHandler(this.button_AnalyseMarket_Click);
            // 
            // groupBox_MarketAnalysis
            // 
            this.groupBox_MarketAnalysis.Controls.Add(this.radioButton_BSE);
            this.groupBox_MarketAnalysis.Controls.Add(this.radioButton_NSE);
            this.groupBox_MarketAnalysis.Controls.Add(this.button_MarketAnalyse);
            this.groupBox_MarketAnalysis.Location = new System.Drawing.Point(1048, 55);
            this.groupBox_MarketAnalysis.Name = "groupBox_MarketAnalysis";
            this.groupBox_MarketAnalysis.Size = new System.Drawing.Size(242, 145);
            this.groupBox_MarketAnalysis.TabIndex = 11;
            this.groupBox_MarketAnalysis.TabStop = false;
            this.groupBox_MarketAnalysis.Text = "Choose Market to Analyse";
            // 
            // radioButton_BSE
            // 
            this.radioButton_BSE.AutoSize = true;
            this.radioButton_BSE.Enabled = false;
            this.radioButton_BSE.Location = new System.Drawing.Point(120, 32);
            this.radioButton_BSE.Name = "radioButton_BSE";
            this.radioButton_BSE.Size = new System.Drawing.Size(67, 24);
            this.radioButton_BSE.TabIndex = 6;
            this.radioButton_BSE.Text = "BSE";
            this.radioButton_BSE.UseVisualStyleBackColor = true;
            // 
            // radioButton_NSE
            // 
            this.radioButton_NSE.AutoSize = true;
            this.radioButton_NSE.Checked = true;
            this.radioButton_NSE.Location = new System.Drawing.Point(28, 32);
            this.radioButton_NSE.Name = "radioButton_NSE";
            this.radioButton_NSE.Size = new System.Drawing.Size(67, 24);
            this.radioButton_NSE.TabIndex = 5;
            this.radioButton_NSE.TabStop = true;
            this.radioButton_NSE.Text = "NSE";
            this.radioButton_NSE.UseVisualStyleBackColor = true;
            // 
            // splitContainer_MarketAnalysis
            // 
            this.splitContainer_MarketAnalysis.Location = new System.Drawing.Point(1048, 289);
            this.splitContainer_MarketAnalysis.Name = "splitContainer_MarketAnalysis";
            this.splitContainer_MarketAnalysis.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_MarketAnalysis.Panel1
            // 
            this.splitContainer_MarketAnalysis.Panel1.Controls.Add(this.dataGridView_MarketAnalysis);
            this.splitContainer_MarketAnalysis.Size = new System.Drawing.Size(640, 642);
            this.splitContainer_MarketAnalysis.SplitterDistance = 555;
            this.splitContainer_MarketAnalysis.SplitterWidth = 5;
            this.splitContainer_MarketAnalysis.TabIndex = 12;
            // 
            // dataGridView_MarketAnalysis
            // 
            this.dataGridView_MarketAnalysis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_MarketAnalysis.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ObserveNow});
            this.dataGridView_MarketAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_MarketAnalysis.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_MarketAnalysis.Name = "dataGridView_MarketAnalysis";
            this.dataGridView_MarketAnalysis.RowTemplate.Height = 28;
            this.dataGridView_MarketAnalysis.Size = new System.Drawing.Size(640, 555);
            this.dataGridView_MarketAnalysis.TabIndex = 0;
            this.dataGridView_MarketAnalysis.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_MarketAnalysis_CellContentClick);
            this.dataGridView_MarketAnalysis.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_MarketAnalysis_ColumnHeaderMouseClick);
            // 
            // ObserveNow
            // 
            this.ObserveNow.HeaderText = "Add to Scan";
            this.ObserveNow.Name = "ObserveNow";
            this.ObserveNow.Text = "Scan Now";
            this.ObserveNow.UseColumnTextForButtonValue = true;
            // 
            // progressBar_MarketAnalysis
            // 
            this.progressBar_MarketAnalysis.Location = new System.Drawing.Point(1404, 71);
            this.progressBar_MarketAnalysis.Maximum = 200;
            this.progressBar_MarketAnalysis.Name = "progressBar_MarketAnalysis";
            this.progressBar_MarketAnalysis.Size = new System.Drawing.Size(294, 42);
            this.progressBar_MarketAnalysis.Step = 1;
            this.progressBar_MarketAnalysis.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_MarketAnalysis.TabIndex = 13;
            // 
            // splitContainer_Scanner
            // 
            this.splitContainer_Scanner.Location = new System.Drawing.Point(16, 47);
            this.splitContainer_Scanner.Name = "splitContainer_Scanner";
            // 
            // splitContainer_Scanner.Panel1
            // 
            this.splitContainer_Scanner.Panel1.Controls.Add(this.dataGridView_Scanner);
            this.splitContainer_Scanner.Size = new System.Drawing.Size(1005, 151);
            this.splitContainer_Scanner.SplitterDistance = 858;
            this.splitContainer_Scanner.TabIndex = 14;
            // 
            // dataGridView_Scanner
            // 
            this.dataGridView_Scanner.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Scanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Scanner.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Scanner.Name = "dataGridView_Scanner";
            this.dataGridView_Scanner.RowTemplate.Height = 28;
            this.dataGridView_Scanner.Size = new System.Drawing.Size(858, 151);
            this.dataGridView_Scanner.TabIndex = 0;
            this.dataGridView_Scanner.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Scanner_CellContentClick);
            this.dataGridView_Scanner.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_Scanner_ColumnHeaderMouseClick);
            this.dataGridView_Scanner.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView_Scanner_RowsAdded);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(226, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "Stocks under Scanner";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 23);
            this.label4.TabIndex = 16;
            this.label4.Text = "Active Orders";
            // 
            // splitContainer_CompletedOrders
            // 
            this.splitContainer_CompletedOrders.Location = new System.Drawing.Point(23, 543);
            this.splitContainer_CompletedOrders.Name = "splitContainer_CompletedOrders";
            // 
            // splitContainer_CompletedOrders.Panel1
            // 
            this.splitContainer_CompletedOrders.Panel1.Controls.Add(this.dataGridView_CompletedOrders);
            this.splitContainer_CompletedOrders.Size = new System.Drawing.Size(998, 197);
            this.splitContainer_CompletedOrders.SplitterDistance = 763;
            this.splitContainer_CompletedOrders.TabIndex = 17;
            // 
            // dataGridView_CompletedOrders
            // 
            this.dataGridView_CompletedOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CompletedOrders.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView_CompletedOrders.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_CompletedOrders.Name = "dataGridView_CompletedOrders";
            this.dataGridView_CompletedOrders.RowTemplate.Height = 28;
            this.dataGridView_CompletedOrders.Size = new System.Drawing.Size(758, 197);
            this.dataGridView_CompletedOrders.TabIndex = 0;
            this.dataGridView_CompletedOrders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CompletedOrders_CellContentClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 516);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 23);
            this.label5.TabIndex = 18;
            this.label5.Text = "Completed Orders";
            // 
            // textBox_Filter
            // 
            this.textBox_Filter.Location = new System.Drawing.Point(1048, 242);
            this.textBox_Filter.Name = "textBox_Filter";
            this.textBox_Filter.Size = new System.Drawing.Size(426, 26);
            this.textBox_Filter.TabIndex = 19;
            // 
            // button_ApplyFilter
            // 
            this.button_ApplyFilter.Location = new System.Drawing.Point(1480, 228);
            this.button_ApplyFilter.Name = "button_ApplyFilter";
            this.button_ApplyFilter.Size = new System.Drawing.Size(98, 42);
            this.button_ApplyFilter.TabIndex = 20;
            this.button_ApplyFilter.Text = "Apply Filter";
            this.button_ApplyFilter.UseVisualStyleBackColor = true;
            this.button_ApplyFilter.Click += new System.EventHandler(this.button_ApplyFilter_Click);
            // 
            // button_ClearFilter
            // 
            this.button_ClearFilter.Location = new System.Drawing.Point(1596, 228);
            this.button_ClearFilter.Name = "button_ClearFilter";
            this.button_ClearFilter.Size = new System.Drawing.Size(94, 42);
            this.button_ClearFilter.TabIndex = 21;
            this.button_ClearFilter.Text = "Clear Filter";
            this.button_ClearFilter.UseVisualStyleBackColor = true;
            this.button_ClearFilter.Click += new System.EventHandler(this.button_ClearFilter_Click);
            // 
            // label_investment
            // 
            this.label_investment.AutoSize = true;
            this.label_investment.Location = new System.Drawing.Point(260, 516);
            this.label_investment.Name = "label_investment";
            this.label_investment.Size = new System.Drawing.Size(51, 20);
            this.label_investment.TabIndex = 22;
            this.label_investment.Text = "label1";
            // 
            // label_profit
            // 
            this.label_profit.AutoSize = true;
            this.label_profit.Location = new System.Drawing.Point(464, 520);
            this.label_profit.Name = "label_profit";
            this.label_profit.Size = new System.Drawing.Size(51, 20);
            this.label_profit.TabIndex = 23;
            this.label_profit.Text = "label1";
            // 
            // OrderID
            // 
            this.OrderID.DataPropertyName = "OrderID";
            this.OrderID.HeaderText = "OrderID";
            this.OrderID.Name = "OrderID";
            // 
            // Ticker
            // 
            this.Ticker.DataPropertyName = "Ticker";
            this.Ticker.HeaderText = "Stock Name";
            this.Ticker.Name = "Ticker";
            // 
            // Units
            // 
            this.Units.DataPropertyName = "PurchaseOrder.Units";
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            // 
            // Purchased_Price
            // 
            this.Purchased_Price.DataPropertyName = "PurchaseOrder.Purchased_Price";
            this.Purchased_Price.HeaderText = "Purchased Price";
            this.Purchased_Price.Name = "Purchased_Price";
            // 
            // CurrentPrice
            // 
            this.CurrentPrice.DataPropertyName = "Current_Price";
            this.CurrentPrice.HeaderText = "Current Price";
            this.CurrentPrice.Name = "CurrentPrice";
            // 
            // Current_Profit
            // 
            this.Current_Profit.DataPropertyName = "Current_Profit";
            this.Current_Profit.HeaderText = "Current Profit";
            this.Current_Profit.Name = "Current_Profit";
            // 
            // StopLoss
            // 
            this.StopLoss.DataPropertyName = "PurchaseOrder.StopLoss";
            this.StopLoss.HeaderText = "Stop Loss Price";
            this.StopLoss.Name = "StopLoss";
            // 
            // Exit_Target
            // 
            this.Exit_Target.DataPropertyName = "PurchaseOrder.ExitPrice";
            this.Exit_Target.HeaderText = "Exit Price";
            this.Exit_Target.Name = "Exit_Target";
            // 
            // BreakEven
            // 
            this.BreakEven.DataPropertyName = "PurchaseOrder.BreakEven";
            this.BreakEven.HeaderText = "Break Even Price";
            this.BreakEven.Name = "BreakEven";
            // 
            // LeastProfitExit
            // 
            this.LeastProfitExit.DataPropertyName = "PurchaseOrder.LeastProfitExit";
            this.LeastProfitExit.HeaderText = "Least Profit Exit Price";
            this.LeastProfitExit.Name = "LeastProfitExit";
            // 
            // Pruchase_Time
            // 
            this.Pruchase_Time.DataPropertyName = "PurchaseOrder.Purchase_Time";
            this.Pruchase_Time.HeaderText = "Purchased Time";
            this.Pruchase_Time.Name = "Pruchase_Time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1718, 962);
            this.Controls.Add(this.label_profit);
            this.Controls.Add(this.label_investment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_ClearFilter);
            this.Controls.Add(this.button_ApplyFilter);
            this.Controls.Add(this.textBox_Filter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.splitContainer_CompletedOrders);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.splitContainer_Scanner);
            this.Controls.Add(this.progressBar_MarketAnalysis);
            this.Controls.Add(this.splitContainer_MarketAnalysis);
            this.Controls.Add(this.groupBox_MarketAnalysis);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.splitContainer_ActiveOrders);
            this.Name = "Form1";
            this.Text = "Day Trade Bot";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer_ActiveOrders.Panel1.ResumeLayout(false);
            this.splitContainer_ActiveOrders.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_ActiveOrders)).EndInit();
            this.splitContainer_ActiveOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ActiveOrderList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox_MarketAnalysis.ResumeLayout(false);
            this.groupBox_MarketAnalysis.PerformLayout();
            this.splitContainer_MarketAnalysis.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_MarketAnalysis)).EndInit();
            this.splitContainer_MarketAnalysis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_MarketAnalysis)).EndInit();
            this.splitContainer_Scanner.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Scanner)).EndInit();
            this.splitContainer_Scanner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Scanner)).EndInit();
            this.splitContainer_CompletedOrders.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_CompletedOrders)).EndInit();
            this.splitContainer_CompletedOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CompletedOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer_ActiveOrders;
        private System.Windows.Forms.DataGridView dataGridView_ActiveOrderList;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private LiveCharts.Wpf.CartesianChart cartesianChart2;
        private System.Windows.Forms.Button button_MarketAnalyse;
        private System.Windows.Forms.GroupBox groupBox_MarketAnalysis;
        private System.Windows.Forms.RadioButton radioButton_BSE;
        private System.Windows.Forms.RadioButton radioButton_NSE;
        private System.Windows.Forms.SplitContainer splitContainer_MarketAnalysis;
        private System.Windows.Forms.DataGridView dataGridView_MarketAnalysis;
        private System.Windows.Forms.ProgressBar progressBar_MarketAnalysis;
        private System.Windows.Forms.SplitContainer splitContainer_Scanner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView_Scanner;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer_CompletedOrders;
        private System.Windows.Forms.DataGridView dataGridView_CompletedOrders;
        private System.Windows.Forms.Label label5;
        private DataGridViewButtonColumn ObserveNow;
        private TextBox textBox_Filter;
        private Button button_ApplyFilter;
        private Button button_ClearFilter;
        private DataGridViewTextBoxColumn Current_Price;
        private Label label_investment;
        private Label label_profit;
        private DataGridViewTextBoxColumn OrderID;
        private DataGridViewTextBoxColumn Ticker;
        private DataGridViewTextBoxColumn Units;
        private DataGridViewTextBoxColumn Purchased_Price;
        private DataGridViewTextBoxColumn CurrentPrice;
        private DataGridViewTextBoxColumn Current_Profit;
        private DataGridViewTextBoxColumn StopLoss;
        private DataGridViewTextBoxColumn Exit_Target;
        private DataGridViewTextBoxColumn BreakEven;
        private DataGridViewTextBoxColumn LeastProfitExit;
        private DataGridViewTextBoxColumn Pruchase_Time;
    }
}

