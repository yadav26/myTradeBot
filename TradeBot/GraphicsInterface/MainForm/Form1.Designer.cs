using System;
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
            this.button_start_trade = new System.Windows.Forms.Button();
            this.comboBox_Algorithms = new System.Windows.Forms.ComboBox();
            this.textBox_ticker = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_stock_num = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView_tradeLists = new System.Windows.Forms.DataGridView();
            this.Exit = new System.Windows.Forms.DataGridViewButtonColumn();
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
            this.stock_buy = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer_CompletedOrders = new System.Windows.Forms.SplitContainer();
            this.dataGridView_CompletedOrders = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tradeLists)).BeginInit();
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
            // button_start_trade
            // 
            this.button_start_trade.Location = new System.Drawing.Point(633, 51);
            this.button_start_trade.Name = "button_start_trade";
            this.button_start_trade.Size = new System.Drawing.Size(87, 35);
            this.button_start_trade.TabIndex = 3;
            this.button_start_trade.Text = "Launch";
            this.button_start_trade.UseVisualStyleBackColor = true;
            this.button_start_trade.Click += new System.EventHandler(this.button_start_Scanner);
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
            this.textBox_ticker.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_ticker.Location = new System.Drawing.Point(74, 50);
            this.textBox_ticker.Name = "textBox_ticker";
            this.textBox_ticker.Size = new System.Drawing.Size(136, 26);
            this.textBox_ticker.TabIndex = 0;
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
            this.textBox_stock_num.TabIndex = 2;
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
            this.splitContainer1.Location = new System.Drawing.Point(18, 315);
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
            this.splitContainer1.Size = new System.Drawing.Size(1005, 163);
            this.splitContainer1.SplitterDistance = 737;
            this.splitContainer1.TabIndex = 8;
            // 
            // dataGridView_tradeLists
            // 
            this.dataGridView_tradeLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_tradeLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Exit});
            this.dataGridView_tradeLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_tradeLists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_tradeLists.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_tradeLists.Name = "dataGridView_tradeLists";
            this.dataGridView_tradeLists.RowTemplate.Height = 28;
            this.dataGridView_tradeLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_tradeLists.Size = new System.Drawing.Size(737, 163);
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
            this.dataGridView1.Size = new System.Drawing.Size(264, 163);
            this.dataGridView1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(26, 676);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.elementHost1);
            this.splitContainer2.Size = new System.Drawing.Size(981, 183);
            this.splitContainer2.SplitterDistance = 896;
            this.splitContainer2.TabIndex = 10;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(896, 183);
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
            this.button_MarketAnalyse.Location = new System.Drawing.Point(24, 84);
            this.button_MarketAnalyse.Name = "button_MarketAnalyse";
            this.button_MarketAnalyse.Size = new System.Drawing.Size(187, 42);
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
            this.groupBox_MarketAnalysis.Location = new System.Drawing.Point(1049, 55);
            this.groupBox_MarketAnalysis.Name = "groupBox_MarketAnalysis";
            this.groupBox_MarketAnalysis.Size = new System.Drawing.Size(242, 144);
            this.groupBox_MarketAnalysis.TabIndex = 11;
            this.groupBox_MarketAnalysis.TabStop = false;
            this.groupBox_MarketAnalysis.Text = "Choose Market to Analyse";
            // 
            // radioButton_BSE
            // 
            this.radioButton_BSE.AutoSize = true;
            this.radioButton_BSE.Enabled = false;
            this.radioButton_BSE.Location = new System.Drawing.Point(120, 33);
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
            this.radioButton_NSE.Location = new System.Drawing.Point(29, 33);
            this.radioButton_NSE.Name = "radioButton_NSE";
            this.radioButton_NSE.Size = new System.Drawing.Size(67, 24);
            this.radioButton_NSE.TabIndex = 5;
            this.radioButton_NSE.TabStop = true;
            this.radioButton_NSE.Text = "NSE";
            this.radioButton_NSE.UseVisualStyleBackColor = true;
            // 
            // splitContainer_MarketAnalysis
            // 
            this.splitContainer_MarketAnalysis.Location = new System.Drawing.Point(1049, 227);
            this.splitContainer_MarketAnalysis.Name = "splitContainer_MarketAnalysis";
            this.splitContainer_MarketAnalysis.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_MarketAnalysis.Panel1
            // 
            this.splitContainer_MarketAnalysis.Panel1.Controls.Add(this.dataGridView_MarketAnalysis);
            this.splitContainer_MarketAnalysis.Size = new System.Drawing.Size(641, 632);
            this.splitContainer_MarketAnalysis.SplitterDistance = 549;
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
            this.dataGridView_MarketAnalysis.Size = new System.Drawing.Size(641, 549);
            this.dataGridView_MarketAnalysis.TabIndex = 0;
            this.dataGridView_MarketAnalysis.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_MarketAnalysis_CellContentClick);
            this.dataGridView_MarketAnalysis.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_MarketAnalysis_ColumnHeaderMouseClick);
            // 
            // ObserveNow
            // 
            this.ObserveNow.HeaderText = "Add to Scan";
            this.ObserveNow.Name = "ObserveNow";
            this.ObserveNow.Text = "Scan Now";
            // 
            // progressBar_MarketAnalysis
            // 
            this.progressBar_MarketAnalysis.Location = new System.Drawing.Point(1404, 71);
            this.progressBar_MarketAnalysis.Maximum = 200;
            this.progressBar_MarketAnalysis.Name = "progressBar_MarketAnalysis";
            this.progressBar_MarketAnalysis.Size = new System.Drawing.Size(294, 41);
            this.progressBar_MarketAnalysis.Step = 1;
            this.progressBar_MarketAnalysis.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_MarketAnalysis.TabIndex = 13;
            // 
            // splitContainer_Scanner
            // 
            this.splitContainer_Scanner.Location = new System.Drawing.Point(18, 118);
            this.splitContainer_Scanner.Name = "splitContainer_Scanner";
            // 
            // splitContainer_Scanner.Panel1
            // 
            this.splitContainer_Scanner.Panel1.Controls.Add(this.dataGridView_Scanner);
            this.splitContainer_Scanner.Size = new System.Drawing.Size(1005, 151);
            this.splitContainer_Scanner.SplitterDistance = 859;
            this.splitContainer_Scanner.TabIndex = 14;
            // 
            // dataGridView_Scanner
            // 
            this.dataGridView_Scanner.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Scanner.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stock_buy});
            this.dataGridView_Scanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Scanner.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Scanner.Name = "dataGridView_Scanner";
            this.dataGridView_Scanner.RowTemplate.Height = 28;
            this.dataGridView_Scanner.Size = new System.Drawing.Size(859, 151);
            this.dataGridView_Scanner.TabIndex = 0;
            this.dataGridView_Scanner.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Scanner_CellContentClick);
            // 
            // stock_buy
            // 
            this.stock_buy.HeaderText = "BUY";
            this.stock_buy.Name = "stock_buy";
            this.stock_buy.Text = "BUY";
            this.stock_buy.ToolTipText = "Click to place the stock purchase order.";
            this.stock_buy.UseColumnTextForButtonValue = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(226, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "Stocks under Scanner";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 23);
            this.label4.TabIndex = 16;
            this.label4.Text = "Active Orders";
            // 
            // splitContainer_CompletedOrders
            // 
            this.splitContainer_CompletedOrders.Location = new System.Drawing.Point(26, 531);
            this.splitContainer_CompletedOrders.Name = "splitContainer_CompletedOrders";
            // 
            // splitContainer_CompletedOrders.Panel1
            // 
            this.splitContainer_CompletedOrders.Panel1.Controls.Add(this.dataGridView_CompletedOrders);
            this.splitContainer_CompletedOrders.Size = new System.Drawing.Size(997, 112);
            this.splitContainer_CompletedOrders.SplitterDistance = 764;
            this.splitContainer_CompletedOrders.TabIndex = 17;
            // 
            // dataGridView_CompletedOrders
            // 
            this.dataGridView_CompletedOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CompletedOrders.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView_CompletedOrders.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_CompletedOrders.Name = "dataGridView_CompletedOrders";
            this.dataGridView_CompletedOrders.RowTemplate.Height = 28;
            this.dataGridView_CompletedOrders.Size = new System.Drawing.Size(758, 112);
            this.dataGridView_CompletedOrders.TabIndex = 0;
            this.dataGridView_CompletedOrders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CompletedOrders_CellContentClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(22, 501);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 23);
            this.label5.TabIndex = 18;
            this.label5.Text = "Completed Orders";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1912, 940);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.splitContainer_CompletedOrders);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.splitContainer_Scanner);
            this.Controls.Add(this.progressBar_MarketAnalysis);
            this.Controls.Add(this.splitContainer_MarketAnalysis);
            this.Controls.Add(this.groupBox_MarketAnalysis);
            this.Controls.Add(this.splitContainer2);
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

        private System.Windows.Forms.Button button_start_trade;
        private System.Windows.Forms.ComboBox comboBox_Algorithms;
        private System.Windows.Forms.TextBox textBox_ticker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_stock_num;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView_tradeLists;
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
        private System.Windows.Forms.DataGridViewButtonColumn Exit;
        private DataGridViewButtonColumn ObserveNow;
        private DataGridViewButtonColumn stock_buy;
    }
}

