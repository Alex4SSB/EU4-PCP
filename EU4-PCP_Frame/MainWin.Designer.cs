﻿namespace EU4_PCP
{
    partial class MainWin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainMS = new DarkUI.Controls.DarkMenuStrip();
            this.DirectoriesM = new System.Windows.Forms.ToolStripMenuItem();
            this.GamePathSM = new System.Windows.Forms.ToolStripMenuItem();
            this.GamePathMTB = new System.Windows.Forms.ToolStripTextBox();
            this.GamePathMB = new System.Windows.Forms.ToolStripMenuItem();
            this.ModPathSM = new System.Windows.Forms.ToolStripMenuItem();
            this.ModPathMTB = new System.Windows.Forms.ToolStripTextBox();
            this.ModPathMB = new System.Windows.Forms.ToolStripMenuItem();
            this.GlobSetM = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoLoadSM = new System.Windows.Forms.ToolStripMenuItem();
            this.DisableLoadMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.RemLoadMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.FullyLoadMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.ProvNamesSM = new System.Windows.Forms.ToolStripMenuItem();
            this.DefinNamesMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.LocNamesMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.DynNamesMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.ModSetM = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicatesSM = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckDupliMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.IgnoreRnwMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.StatsM = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadingTimeML = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadingValueML = new System.Windows.Forms.ToolStripMenuItem();
            this.ColorPickerGB = new DarkUI.Controls.DarkGroupBox();
            this.AddProvB = new DarkUI.Controls.DarkButton();
            this.TempL = new System.Windows.Forms.Label();
            this.GenColL = new DarkUI.Controls.DarkLabel();
            this.GenCM = new DarkUI.Controls.DarkContextMenu();
            this.NewColorMB = new System.Windows.Forms.ToolStripMenuItem();
            this.NextProvNameL = new DarkUI.Controls.DarkLabel();
            this.NextProvNameTB = new DarkUI.Controls.DarkTextBox();
            this.BlueL = new DarkUI.Controls.DarkLabel();
            this.BlueTB = new DarkUI.Controls.DarkTextBox();
            this.GreenL = new DarkUI.Controls.DarkLabel();
            this.GreenTB = new DarkUI.Controls.DarkTextBox();
            this.RedL = new DarkUI.Controls.DarkLabel();
            this.RedTB = new DarkUI.Controls.DarkTextBox();
            this.NextProvNumberL = new DarkUI.Controls.DarkLabel();
            this.NextProvNumberTB = new DarkUI.Controls.DarkTextBox();
            this.ProvTable = new System.Windows.Forms.DataGridView();
            this.ProvColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvRed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvGreen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvBlue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvCM = new DarkUI.Controls.DarkContextMenu();
            this.ChangeColorSM = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectInPickerMB = new System.Windows.Forms.ToolStripMenuItem();
            this.ProvTableSB = new DarkUI.Controls.DarkScrollBar();
            this.ModSelCB = new DarkUI.Controls.DarkComboBox();
            this.ModSelL = new DarkUI.Controls.DarkLabel();
            this.GameInfoGB = new DarkUI.Controls.DarkGroupBox();
            this.GameBookmarkL = new DarkUI.Controls.DarkLabel();
            this.GameBookmarkCB = new DarkUI.Controls.DarkComboBox();
            this.GameStartDateL = new DarkUI.Controls.DarkLabel();
            this.GameStartDateTB = new DarkUI.Controls.DarkTextBox();
            this.GameMaxProvL = new DarkUI.Controls.DarkLabel();
            this.GameMaxProvTB = new DarkUI.Controls.DarkTextBox();
            this.GameProvShownL = new DarkUI.Controls.DarkLabel();
            this.GameProvShownTB = new DarkUI.Controls.DarkTextBox();
            this.GameProvCountL = new DarkUI.Controls.DarkLabel();
            this.GameProvCountTB = new DarkUI.Controls.DarkTextBox();
            this.ModInfoGB = new DarkUI.Controls.DarkGroupBox();
            this.ModBookmarkL = new DarkUI.Controls.DarkLabel();
            this.ModBookmarkCB = new DarkUI.Controls.DarkComboBox();
            this.ModStartDateL = new DarkUI.Controls.DarkLabel();
            this.ModStartDateTB = new DarkUI.Controls.DarkTextBox();
            this.ModMaxPRovL = new DarkUI.Controls.DarkLabel();
            this.ModMaxProvTB = new DarkUI.Controls.DarkTextBox();
            this.ModProvShownL = new DarkUI.Controls.DarkLabel();
            this.ModProvShownTB = new DarkUI.Controls.DarkTextBox();
            this.ModProvCountL = new DarkUI.Controls.DarkLabel();
            this.ModProvCountTB = new DarkUI.Controls.DarkTextBox();
            this.BrowserFBD = new System.Windows.Forms.FolderBrowserDialog();
            this.TextBoxTT = new System.Windows.Forms.ToolTip(this.components);
            this.ShowAllProvsMCB = new System.Windows.Forms.ToolStripMenuItem();
            this.GlobSetTSS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMS.SuspendLayout();
            this.ColorPickerGB.SuspendLayout();
            this.GenCM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProvTable)).BeginInit();
            this.ProvCM.SuspendLayout();
            this.GameInfoGB.SuspendLayout();
            this.ModInfoGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMS
            // 
            this.MainMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.MainMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.MainMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DirectoriesM,
            this.GlobSetM,
            this.ModSetM,
            this.StatsM});
            this.MainMS.Location = new System.Drawing.Point(0, 0);
            this.MainMS.Name = "MainMS";
            this.MainMS.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.MainMS.Size = new System.Drawing.Size(651, 24);
            this.MainMS.TabIndex = 0;
            this.MainMS.Text = "darkMenuStrip1";
            // 
            // DirectoriesM
            // 
            this.DirectoriesM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DirectoriesM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GamePathSM,
            this.ModPathSM});
            this.DirectoriesM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DirectoriesM.Name = "DirectoriesM";
            this.DirectoriesM.Size = new System.Drawing.Size(123, 20);
            this.DirectoriesM.Text = "Working Directories";
            // 
            // GamePathSM
            // 
            this.GamePathSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.GamePathSM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GamePathMTB,
            this.GamePathMB});
            this.GamePathSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GamePathSM.Name = "GamePathSM";
            this.GamePathSM.Size = new System.Drawing.Size(180, 22);
            this.GamePathSM.Text = "Game";
            // 
            // GamePathMTB
            // 
            this.GamePathMTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.GamePathMTB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.GamePathMTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GamePathMTB.Name = "GamePathMTB";
            this.GamePathMTB.ReadOnly = true;
            this.GamePathMTB.Size = new System.Drawing.Size(180, 23);
            // 
            // GamePathMB
            // 
            this.GamePathMB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.GamePathMB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GamePathMB.Name = "GamePathMB";
            this.GamePathMB.Size = new System.Drawing.Size(240, 22);
            this.GamePathMB.Text = "Browse";
            // 
            // ModPathSM
            // 
            this.ModPathSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ModPathSM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModPathMTB,
            this.ModPathMB});
            this.ModPathSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModPathSM.Name = "ModPathSM";
            this.ModPathSM.Size = new System.Drawing.Size(180, 22);
            this.ModPathSM.Text = "Mod";
            // 
            // ModPathMTB
            // 
            this.ModPathMTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ModPathMTB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ModPathMTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModPathMTB.Name = "ModPathMTB";
            this.ModPathMTB.ReadOnly = true;
            this.ModPathMTB.Size = new System.Drawing.Size(180, 23);
            // 
            // ModPathMB
            // 
            this.ModPathMB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ModPathMB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModPathMB.Name = "ModPathMB";
            this.ModPathMB.Size = new System.Drawing.Size(240, 22);
            this.ModPathMB.Text = "Browse";
            // 
            // GlobSetM
            // 
            this.GlobSetM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.GlobSetM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoLoadSM,
            this.ProvNamesSM,
            this.GlobSetTSS1,
            this.ShowAllProvsMCB});
            this.GlobSetM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GlobSetM.Name = "GlobSetM";
            this.GlobSetM.Size = new System.Drawing.Size(98, 20);
            this.GlobSetM.Text = "Global Settings";
            this.GlobSetM.DropDownOpening += new System.EventHandler(this.Menu_DropDownOpening);
            // 
            // AutoLoadSM
            // 
            this.AutoLoadSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.AutoLoadSM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisableLoadMCB,
            this.RemLoadMCB,
            this.FullyLoadMCB});
            this.AutoLoadSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.AutoLoadSM.Name = "AutoLoadSM";
            this.AutoLoadSM.Size = new System.Drawing.Size(180, 22);
            this.AutoLoadSM.Tag = "";
            this.AutoLoadSM.Text = "Auto-Load";
            // 
            // DisableLoadMCB
            // 
            this.DisableLoadMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DisableLoadMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DisableLoadMCB.Image = global::EU4_PCP.Properties.Resources.UncheckedRadio;
            this.DisableLoadMCB.Name = "DisableLoadMCB";
            this.DisableLoadMCB.Size = new System.Drawing.Size(180, 22);
            this.DisableLoadMCB.Text = "Disabled";
            this.DisableLoadMCB.Click += new System.EventHandler(this.DisableLoadMCB_Click);
            // 
            // RemLoadMCB
            // 
            this.RemLoadMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.RemLoadMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.RemLoadMCB.Image = global::EU4_PCP.Properties.Resources.UncheckedRadio;
            this.RemLoadMCB.Name = "RemLoadMCB";
            this.RemLoadMCB.Size = new System.Drawing.Size(180, 22);
            this.RemLoadMCB.Text = "Remember Paths";
            this.RemLoadMCB.Click += new System.EventHandler(this.RemLoadMCB_Click);
            // 
            // FullyLoadMCB
            // 
            this.FullyLoadMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.FullyLoadMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.FullyLoadMCB.Image = global::EU4_PCP.Properties.Resources.UncheckedRadio;
            this.FullyLoadMCB.Name = "FullyLoadMCB";
            this.FullyLoadMCB.Size = new System.Drawing.Size(180, 22);
            this.FullyLoadMCB.Text = "Load Last Mod";
            this.FullyLoadMCB.Click += new System.EventHandler(this.FullyLoadMCB_Click);
            // 
            // ProvNamesSM
            // 
            this.ProvNamesSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ProvNamesSM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DefinNamesMCB,
            this.LocNamesMCB,
            this.DynNamesMCB});
            this.ProvNamesSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ProvNamesSM.Name = "ProvNamesSM";
            this.ProvNamesSM.Size = new System.Drawing.Size(180, 22);
            this.ProvNamesSM.Text = "Province Names";
            this.ProvNamesSM.DropDownOpening += new System.EventHandler(this.ProvNamesSM_DropDownOpening);
            // 
            // DefinNamesMCB
            // 
            this.DefinNamesMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DefinNamesMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DefinNamesMCB.Image = global::EU4_PCP.Properties.Resources.UncheckedBox;
            this.DefinNamesMCB.Name = "DefinNamesMCB";
            this.DefinNamesMCB.Size = new System.Drawing.Size(180, 22);
            this.DefinNamesMCB.Text = "From Definition";
            this.DefinNamesMCB.Click += new System.EventHandler(this.DefinNamesMCB_Click);
            // 
            // LocNamesMCB
            // 
            this.LocNamesMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.LocNamesMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.LocNamesMCB.Image = ((System.Drawing.Image)(resources.GetObject("LocNamesMCB.Image")));
            this.LocNamesMCB.Name = "LocNamesMCB";
            this.LocNamesMCB.Size = new System.Drawing.Size(180, 22);
            this.LocNamesMCB.Text = "From Localisation";
            this.LocNamesMCB.Click += new System.EventHandler(this.LocNamesMCB_Click);
            // 
            // DynNamesMCB
            // 
            this.DynNamesMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DynNamesMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DynNamesMCB.Image = ((System.Drawing.Image)(resources.GetObject("DynNamesMCB.Image")));
            this.DynNamesMCB.Name = "DynNamesMCB";
            this.DynNamesMCB.Size = new System.Drawing.Size(180, 22);
            this.DynNamesMCB.Text = "Dynamic";
            this.DynNamesMCB.Click += new System.EventHandler(this.DynNamesMCB_Click);
            // 
            // ModSetM
            // 
            this.ModSetM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ModSetM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DuplicatesSM});
            this.ModSetM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModSetM.Name = "ModSetM";
            this.ModSetM.Size = new System.Drawing.Size(89, 20);
            this.ModSetM.Text = "Mod Settings";
            this.ModSetM.DropDownOpening += new System.EventHandler(this.Menu_DropDownOpening);
            // 
            // DuplicatesSM
            // 
            this.DuplicatesSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DuplicatesSM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckDupliMCB,
            this.IgnoreRnwMCB});
            this.DuplicatesSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DuplicatesSM.Name = "DuplicatesSM";
            this.DuplicatesSM.Size = new System.Drawing.Size(180, 22);
            this.DuplicatesSM.Text = "Color duplicates";
            // 
            // CheckDupliMCB
            // 
            this.CheckDupliMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.CheckDupliMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.CheckDupliMCB.Image = ((System.Drawing.Image)(resources.GetObject("CheckDupliMCB.Image")));
            this.CheckDupliMCB.Name = "CheckDupliMCB";
            this.CheckDupliMCB.Size = new System.Drawing.Size(248, 22);
            this.CheckDupliMCB.Text = "Check Color Duplicates";
            this.CheckDupliMCB.CheckedChanged += new System.EventHandler(this.CheckDupliMCB_CheckedChanged);
            this.CheckDupliMCB.Click += new System.EventHandler(this.CheckDupliMCB_Click);
            // 
            // IgnoreRnwMCB
            // 
            this.IgnoreRnwMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.IgnoreRnwMCB.Enabled = false;
            this.IgnoreRnwMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.IgnoreRnwMCB.Image = ((System.Drawing.Image)(resources.GetObject("IgnoreRnwMCB.Image")));
            this.IgnoreRnwMCB.Name = "IgnoreRnwMCB";
            this.IgnoreRnwMCB.Size = new System.Drawing.Size(248, 22);
            this.IgnoreRnwMCB.Text = "Ignore RNW && Unused Provinces";
            this.IgnoreRnwMCB.Click += new System.EventHandler(this.IgnoreRnwMCB_Click);
            // 
            // StatsM
            // 
            this.StatsM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.StatsM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadingTimeML,
            this.LoadingValueML});
            this.StatsM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.StatsM.Name = "StatsM";
            this.StatsM.Size = new System.Drawing.Size(65, 20);
            this.StatsM.Text = "Statistics";
            this.StatsM.DropDownOpening += new System.EventHandler(this.StatsM_DropDownOpening);
            // 
            // LoadingTimeML
            // 
            this.LoadingTimeML.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.LoadingTimeML.Enabled = false;
            this.LoadingTimeML.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.LoadingTimeML.Name = "LoadingTimeML";
            this.LoadingTimeML.Size = new System.Drawing.Size(147, 22);
            this.LoadingTimeML.Text = "Loading time:";
            // 
            // LoadingValueML
            // 
            this.LoadingValueML.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.LoadingValueML.Enabled = false;
            this.LoadingValueML.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.LoadingValueML.Name = "LoadingValueML";
            this.LoadingValueML.Size = new System.Drawing.Size(147, 22);
            this.LoadingValueML.Text = "xxxx mSec";
            // 
            // ColorPickerGB
            // 
            this.ColorPickerGB.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ColorPickerGB.Controls.Add(this.AddProvB);
            this.ColorPickerGB.Controls.Add(this.TempL);
            this.ColorPickerGB.Controls.Add(this.GenColL);
            this.ColorPickerGB.Controls.Add(this.NextProvNameL);
            this.ColorPickerGB.Controls.Add(this.NextProvNameTB);
            this.ColorPickerGB.Controls.Add(this.BlueL);
            this.ColorPickerGB.Controls.Add(this.BlueTB);
            this.ColorPickerGB.Controls.Add(this.GreenL);
            this.ColorPickerGB.Controls.Add(this.GreenTB);
            this.ColorPickerGB.Controls.Add(this.RedL);
            this.ColorPickerGB.Controls.Add(this.RedTB);
            this.ColorPickerGB.Controls.Add(this.NextProvNumberL);
            this.ColorPickerGB.Controls.Add(this.NextProvNumberTB);
            this.ColorPickerGB.Enabled = false;
            this.ColorPickerGB.Location = new System.Drawing.Point(418, 316);
            this.ColorPickerGB.Name = "ColorPickerGB";
            this.ColorPickerGB.Size = new System.Drawing.Size(221, 214);
            this.ColorPickerGB.TabIndex = 4;
            this.ColorPickerGB.TabStop = false;
            this.ColorPickerGB.Text = "Color Picker";
            // 
            // AddProvB
            // 
            this.AddProvB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddProvB.Enabled = false;
            this.AddProvB.ForeColor = System.Drawing.Color.Gainsboro;
            this.AddProvB.Location = new System.Drawing.Point(56, 175);
            this.AddProvB.Name = "AddProvB";
            this.AddProvB.Padding = new System.Windows.Forms.Padding(6);
            this.AddProvB.Size = new System.Drawing.Size(109, 25);
            this.AddProvB.TabIndex = 17;
            this.AddProvB.Text = "Add Province";
            this.AddProvB.Click += new System.EventHandler(this.AddProvB_Click);
            // 
            // TempL
            // 
            this.TempL.AutoSize = true;
            this.TempL.Location = new System.Drawing.Point(25, 182);
            this.TempL.Name = "TempL";
            this.TempL.Size = new System.Drawing.Size(0, 15);
            this.TempL.TabIndex = 41;
            this.TempL.Visible = false;
            // 
            // GenColL
            // 
            this.GenColL.BackColor = System.Drawing.Color.Black;
            this.GenColL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GenColL.ContextMenuStrip = this.GenCM;
            this.GenColL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GenColL.Location = new System.Drawing.Point(150, 84);
            this.GenColL.Name = "GenColL";
            this.GenColL.Size = new System.Drawing.Size(65, 23);
            this.GenColL.TabIndex = 18;
            this.GenColL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GenCM
            // 
            this.GenCM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.GenCM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GenCM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewColorMB});
            this.GenCM.Name = "darkContextMenu1";
            this.GenCM.Size = new System.Drawing.Size(131, 26);
            // 
            // NewColorMB
            // 
            this.NewColorMB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.NewColorMB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.NewColorMB.Name = "NewColorMB";
            this.NewColorMB.Size = new System.Drawing.Size(130, 22);
            this.NewColorMB.Text = "New Color";
            this.NewColorMB.Click += new System.EventHandler(this.NewColorMB_Click);
            // 
            // NextProvNameL
            // 
            this.NextProvNameL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.NextProvNameL.Location = new System.Drawing.Point(6, 120);
            this.NextProvNameL.Name = "NextProvNameL";
            this.NextProvNameL.Size = new System.Drawing.Size(209, 15);
            this.NextProvNameL.TabIndex = 16;
            this.NextProvNameL.Text = "Province Name";
            this.NextProvNameL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NextProvNameTB
            // 
            this.NextProvNameTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.NextProvNameTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NextProvNameTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.NextProvNameTB.Location = new System.Drawing.Point(6, 138);
            this.NextProvNameTB.Name = "NextProvNameTB";
            this.NextProvNameTB.Size = new System.Drawing.Size(209, 23);
            this.NextProvNameTB.TabIndex = 15;
            this.NextProvNameTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NextProvNameTB.TextChanged += new System.EventHandler(this.NextProvNameTB_TextChanged);
            // 
            // BlueL
            // 
            this.BlueL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.BlueL.Location = new System.Drawing.Point(102, 66);
            this.BlueL.Name = "BlueL";
            this.BlueL.Size = new System.Drawing.Size(42, 15);
            this.BlueL.TabIndex = 14;
            this.BlueL.Text = "Blue";
            this.BlueL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BlueTB
            // 
            this.BlueTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.BlueTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BlueTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.BlueTB.Location = new System.Drawing.Point(102, 84);
            this.BlueTB.Name = "BlueTB";
            this.BlueTB.ReadOnly = true;
            this.BlueTB.Size = new System.Drawing.Size(42, 23);
            this.BlueTB.TabIndex = 13;
            this.BlueTB.Text = "0";
            this.BlueTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GreenL
            // 
            this.GreenL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GreenL.Location = new System.Drawing.Point(54, 66);
            this.GreenL.Name = "GreenL";
            this.GreenL.Size = new System.Drawing.Size(42, 15);
            this.GreenL.TabIndex = 12;
            this.GreenL.Text = "Green";
            this.GreenL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GreenTB
            // 
            this.GreenTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.GreenTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GreenTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GreenTB.Location = new System.Drawing.Point(54, 84);
            this.GreenTB.Name = "GreenTB";
            this.GreenTB.ReadOnly = true;
            this.GreenTB.Size = new System.Drawing.Size(42, 23);
            this.GreenTB.TabIndex = 11;
            this.GreenTB.Text = "0";
            this.GreenTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RedL
            // 
            this.RedL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.RedL.Location = new System.Drawing.Point(6, 66);
            this.RedL.Name = "RedL";
            this.RedL.Size = new System.Drawing.Size(42, 15);
            this.RedL.TabIndex = 10;
            this.RedL.Text = "Red";
            this.RedL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RedTB
            // 
            this.RedTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.RedTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RedTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.RedTB.Location = new System.Drawing.Point(6, 84);
            this.RedTB.Name = "RedTB";
            this.RedTB.ReadOnly = true;
            this.RedTB.Size = new System.Drawing.Size(42, 23);
            this.RedTB.TabIndex = 9;
            this.RedTB.Text = "0";
            this.RedTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NextProvNumberL
            // 
            this.NextProvNumberL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.NextProvNumberL.Location = new System.Drawing.Point(73, 19);
            this.NextProvNumberL.Name = "NextProvNumberL";
            this.NextProvNumberL.Size = new System.Drawing.Size(75, 15);
            this.NextProvNumberL.TabIndex = 6;
            this.NextProvNumberL.Text = "Province ID";
            this.NextProvNumberL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NextProvNumberTB
            // 
            this.NextProvNumberTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.NextProvNumberTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NextProvNumberTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.NextProvNumberTB.Location = new System.Drawing.Point(73, 37);
            this.NextProvNumberTB.Name = "NextProvNumberTB";
            this.NextProvNumberTB.ReadOnly = true;
            this.NextProvNumberTB.Size = new System.Drawing.Size(75, 23);
            this.NextProvNumberTB.TabIndex = 5;
            this.NextProvNumberTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProvTable
            // 
            this.ProvTable.AllowUserToAddRows = false;
            this.ProvTable.AllowUserToDeleteRows = false;
            this.ProvTable.AllowUserToResizeColumns = false;
            this.ProvTable.AllowUserToResizeRows = false;
            this.ProvTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ProvTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProvTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ProvTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ProvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProvTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProvColor,
            this.ProvNumber,
            this.ProvName,
            this.ProvRed,
            this.ProvGreen,
            this.ProvBlue});
            this.ProvTable.ContextMenuStrip = this.ProvCM;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(110)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvTable.DefaultCellStyle = dataGridViewCellStyle3;
            this.ProvTable.EnableHeadersVisualStyles = false;
            this.ProvTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.ProvTable.Location = new System.Drawing.Point(12, 27);
            this.ProvTable.MultiSelect = false;
            this.ProvTable.Name = "ProvTable";
            this.ProvTable.ReadOnly = true;
            this.ProvTable.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.MenuBar;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ProvTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.ProvTable.RowHeadersVisible = false;
            this.ProvTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ProvTable.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ProvTable.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ProvTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ProvTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProvTable.Size = new System.Drawing.Size(400, 503);
            this.ProvTable.TabIndex = 31;
            this.ProvTable.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ProvTable_Scroll);
            this.ProvTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProvTable_MouseDown);
            // 
            // ProvColor
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            this.ProvColor.DefaultCellStyle = dataGridViewCellStyle2;
            this.ProvColor.HeaderText = "";
            this.ProvColor.Name = "ProvColor";
            this.ProvColor.ReadOnly = true;
            this.ProvColor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvColor.Width = 40;
            // 
            // ProvNumber
            // 
            this.ProvNumber.HeaderText = "ID";
            this.ProvNumber.Name = "ProvNumber";
            this.ProvNumber.ReadOnly = true;
            this.ProvNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvNumber.Width = 40;
            // 
            // ProvName
            // 
            this.ProvName.HeaderText = "Name";
            this.ProvName.Name = "ProvName";
            this.ProvName.ReadOnly = true;
            this.ProvName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvName.Width = 165;
            // 
            // ProvRed
            // 
            this.ProvRed.HeaderText = "Red";
            this.ProvRed.Name = "ProvRed";
            this.ProvRed.ReadOnly = true;
            this.ProvRed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvRed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvRed.Width = 45;
            // 
            // ProvGreen
            // 
            this.ProvGreen.HeaderText = "Green";
            this.ProvGreen.Name = "ProvGreen";
            this.ProvGreen.ReadOnly = true;
            this.ProvGreen.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvGreen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvGreen.Width = 45;
            // 
            // ProvBlue
            // 
            this.ProvBlue.HeaderText = "Blue";
            this.ProvBlue.Name = "ProvBlue";
            this.ProvBlue.ReadOnly = true;
            this.ProvBlue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProvBlue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvBlue.Width = 45;
            // 
            // ProvCM
            // 
            this.ProvCM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ProvCM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ProvCM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeColorSM});
            this.ProvCM.Name = "ProvCM";
            this.ProvCM.Size = new System.Drawing.Size(146, 26);
            // 
            // ChangeColorSM
            // 
            this.ChangeColorSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ChangeColorSM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectInPickerMB});
            this.ChangeColorSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ChangeColorSM.Name = "ChangeColorSM";
            this.ChangeColorSM.Size = new System.Drawing.Size(145, 22);
            this.ChangeColorSM.Text = "Change color";
            // 
            // SelectInPickerMB
            // 
            this.SelectInPickerMB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.SelectInPickerMB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SelectInPickerMB.Name = "SelectInPickerMB";
            this.SelectInPickerMB.Size = new System.Drawing.Size(205, 22);
            this.SelectInPickerMB.Text = "Select in the Color Picker";
            this.SelectInPickerMB.Click += new System.EventHandler(this.SelectInPickerMB_Click);
            // 
            // ProvTableSB
            // 
            this.ProvTableSB.Location = new System.Drawing.Point(394, 27);
            this.ProvTableSB.Maximum = 78;
            this.ProvTableSB.Name = "ProvTableSB";
            this.ProvTableSB.Size = new System.Drawing.Size(18, 503);
            this.ProvTableSB.TabIndex = 32;
            this.ProvTableSB.Text = "darkScrollBar1";
            this.ProvTableSB.ViewSize = 1;
            this.ProvTableSB.Visible = false;
            this.ProvTableSB.ValueChanged += new System.EventHandler<DarkUI.Controls.ScrollValueEventArgs>(this.ProvTableSB_ValueChanged);
            this.ProvTableSB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ProvTableSB_MouseMove);
            // 
            // ModSelCB
            // 
            this.ModSelCB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ModSelCB.Location = new System.Drawing.Point(454, 166);
            this.ModSelCB.MaxDropDownItems = 15;
            this.ModSelCB.Name = "ModSelCB";
            this.ModSelCB.Size = new System.Drawing.Size(161, 24);
            this.ModSelCB.TabIndex = 36;
            this.ModSelCB.DropDown += new System.EventHandler(this.ModSelCB_DropDown);
            this.ModSelCB.SelectedIndexChanged += new System.EventHandler(this.ModSelCB_SelectedIndexChanged);
            this.ModSelCB.MouseHover += new System.EventHandler(this.CB_MouseHover);
            // 
            // ModSelL
            // 
            this.ModSelL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModSelL.Location = new System.Drawing.Point(454, 148);
            this.ModSelL.Name = "ModSelL";
            this.ModSelL.Size = new System.Drawing.Size(161, 15);
            this.ModSelL.TabIndex = 37;
            this.ModSelL.Text = "Mod";
            this.ModSelL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameInfoGB
            // 
            this.GameInfoGB.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.GameInfoGB.Controls.Add(this.GameBookmarkL);
            this.GameInfoGB.Controls.Add(this.GameBookmarkCB);
            this.GameInfoGB.Controls.Add(this.GameStartDateL);
            this.GameInfoGB.Controls.Add(this.GameStartDateTB);
            this.GameInfoGB.Controls.Add(this.GameMaxProvL);
            this.GameInfoGB.Controls.Add(this.GameMaxProvTB);
            this.GameInfoGB.Controls.Add(this.GameProvShownL);
            this.GameInfoGB.Controls.Add(this.GameProvShownTB);
            this.GameInfoGB.Controls.Add(this.GameProvCountL);
            this.GameInfoGB.Controls.Add(this.GameProvCountTB);
            this.GameInfoGB.Location = new System.Drawing.Point(418, 27);
            this.GameInfoGB.Name = "GameInfoGB";
            this.GameInfoGB.Size = new System.Drawing.Size(221, 113);
            this.GameInfoGB.TabIndex = 38;
            this.GameInfoGB.TabStop = false;
            this.GameInfoGB.Text = "Game";
            // 
            // GameBookmarkL
            // 
            this.GameBookmarkL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameBookmarkL.Location = new System.Drawing.Point(6, 65);
            this.GameBookmarkL.Name = "GameBookmarkL";
            this.GameBookmarkL.Size = new System.Drawing.Size(122, 16);
            this.GameBookmarkL.TabIndex = 38;
            this.GameBookmarkL.Text = "Bookmark";
            this.GameBookmarkL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameBookmarkCB
            // 
            this.GameBookmarkCB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.GameBookmarkCB.FormattingEnabled = true;
            this.GameBookmarkCB.Location = new System.Drawing.Point(6, 83);
            this.GameBookmarkCB.MaxDropDownItems = 15;
            this.GameBookmarkCB.Name = "GameBookmarkCB";
            this.GameBookmarkCB.Size = new System.Drawing.Size(122, 24);
            this.GameBookmarkCB.TabIndex = 37;
            this.GameBookmarkCB.DropDown += new System.EventHandler(this.BookmarkCB_DropDown);
            this.GameBookmarkCB.SelectedIndexChanged += new System.EventHandler(this.GameBookmarkCB_SelectedIndexChanged);
            this.GameBookmarkCB.MouseHover += new System.EventHandler(this.CB_MouseHover);
            // 
            // GameStartDateL
            // 
            this.GameStartDateL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameStartDateL.Location = new System.Drawing.Point(134, 66);
            this.GameStartDateL.Name = "GameStartDateL";
            this.GameStartDateL.Size = new System.Drawing.Size(81, 15);
            this.GameStartDateL.TabIndex = 10;
            this.GameStartDateL.Text = "Start Date";
            this.GameStartDateL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameStartDateTB
            // 
            this.GameStartDateTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.GameStartDateTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GameStartDateTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameStartDateTB.Location = new System.Drawing.Point(134, 84);
            this.GameStartDateTB.Name = "GameStartDateTB";
            this.GameStartDateTB.ReadOnly = true;
            this.GameStartDateTB.Size = new System.Drawing.Size(81, 23);
            this.GameStartDateTB.TabIndex = 9;
            this.GameStartDateTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GameStartDateTB.MouseHover += new System.EventHandler(this.StartDateTB_MouseHover);
            // 
            // GameMaxProvL
            // 
            this.GameMaxProvL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameMaxProvL.Location = new System.Drawing.Point(150, 19);
            this.GameMaxProvL.Name = "GameMaxProvL";
            this.GameMaxProvL.Size = new System.Drawing.Size(65, 15);
            this.GameMaxProvL.TabIndex = 8;
            this.GameMaxProvL.Text = "Max";
            this.GameMaxProvL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameMaxProvTB
            // 
            this.GameMaxProvTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.GameMaxProvTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GameMaxProvTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameMaxProvTB.Location = new System.Drawing.Point(150, 37);
            this.GameMaxProvTB.Name = "GameMaxProvTB";
            this.GameMaxProvTB.ReadOnly = true;
            this.GameMaxProvTB.Size = new System.Drawing.Size(65, 23);
            this.GameMaxProvTB.TabIndex = 7;
            this.GameMaxProvTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GameMaxProvTB.MouseHover += new System.EventHandler(this.MaxProvTB_MouseHover);
            // 
            // GameProvShownL
            // 
            this.GameProvShownL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameProvShownL.Location = new System.Drawing.Point(77, 19);
            this.GameProvShownL.Name = "GameProvShownL";
            this.GameProvShownL.Size = new System.Drawing.Size(67, 15);
            this.GameProvShownL.TabIndex = 6;
            this.GameProvShownL.Text = "Shown";
            this.GameProvShownL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameProvShownTB
            // 
            this.GameProvShownTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.GameProvShownTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GameProvShownTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameProvShownTB.Location = new System.Drawing.Point(77, 37);
            this.GameProvShownTB.Name = "GameProvShownTB";
            this.GameProvShownTB.ReadOnly = true;
            this.GameProvShownTB.Size = new System.Drawing.Size(67, 23);
            this.GameProvShownTB.TabIndex = 5;
            this.GameProvShownTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GameProvCountL
            // 
            this.GameProvCountL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameProvCountL.Location = new System.Drawing.Point(6, 19);
            this.GameProvCountL.Name = "GameProvCountL";
            this.GameProvCountL.Size = new System.Drawing.Size(65, 15);
            this.GameProvCountL.TabIndex = 4;
            this.GameProvCountL.Text = "Provinces";
            this.GameProvCountL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameProvCountTB
            // 
            this.GameProvCountTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.GameProvCountTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GameProvCountTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GameProvCountTB.Location = new System.Drawing.Point(6, 37);
            this.GameProvCountTB.Name = "GameProvCountTB";
            this.GameProvCountTB.ReadOnly = true;
            this.GameProvCountTB.Size = new System.Drawing.Size(65, 23);
            this.GameProvCountTB.TabIndex = 3;
            this.GameProvCountTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ModInfoGB
            // 
            this.ModInfoGB.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ModInfoGB.Controls.Add(this.ModBookmarkL);
            this.ModInfoGB.Controls.Add(this.ModBookmarkCB);
            this.ModInfoGB.Controls.Add(this.ModStartDateL);
            this.ModInfoGB.Controls.Add(this.ModStartDateTB);
            this.ModInfoGB.Controls.Add(this.ModMaxPRovL);
            this.ModInfoGB.Controls.Add(this.ModMaxProvTB);
            this.ModInfoGB.Controls.Add(this.ModProvShownL);
            this.ModInfoGB.Controls.Add(this.ModProvShownTB);
            this.ModInfoGB.Controls.Add(this.ModProvCountL);
            this.ModInfoGB.Controls.Add(this.ModProvCountTB);
            this.ModInfoGB.Location = new System.Drawing.Point(418, 197);
            this.ModInfoGB.Name = "ModInfoGB";
            this.ModInfoGB.Size = new System.Drawing.Size(221, 113);
            this.ModInfoGB.TabIndex = 39;
            this.ModInfoGB.TabStop = false;
            this.ModInfoGB.Text = "Mod";
            this.ModInfoGB.Visible = false;
            // 
            // ModBookmarkL
            // 
            this.ModBookmarkL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModBookmarkL.Location = new System.Drawing.Point(6, 65);
            this.ModBookmarkL.Name = "ModBookmarkL";
            this.ModBookmarkL.Size = new System.Drawing.Size(122, 16);
            this.ModBookmarkL.TabIndex = 38;
            this.ModBookmarkL.Text = "Bookmark";
            this.ModBookmarkL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModBookmarkCB
            // 
            this.ModBookmarkCB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ModBookmarkCB.FormattingEnabled = true;
            this.ModBookmarkCB.Location = new System.Drawing.Point(6, 83);
            this.ModBookmarkCB.MaxDropDownItems = 15;
            this.ModBookmarkCB.Name = "ModBookmarkCB";
            this.ModBookmarkCB.Size = new System.Drawing.Size(122, 24);
            this.ModBookmarkCB.TabIndex = 37;
            this.ModBookmarkCB.DropDown += new System.EventHandler(this.BookmarkCB_DropDown);
            this.ModBookmarkCB.SelectedIndexChanged += new System.EventHandler(this.ModBookmarkCB_SelectedIndexChanged);
            this.ModBookmarkCB.MouseHover += new System.EventHandler(this.CB_MouseHover);
            // 
            // ModStartDateL
            // 
            this.ModStartDateL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModStartDateL.Location = new System.Drawing.Point(134, 66);
            this.ModStartDateL.Name = "ModStartDateL";
            this.ModStartDateL.Size = new System.Drawing.Size(81, 15);
            this.ModStartDateL.TabIndex = 10;
            this.ModStartDateL.Text = "Start Date";
            this.ModStartDateL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModStartDateTB
            // 
            this.ModStartDateTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.ModStartDateTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ModStartDateTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModStartDateTB.Location = new System.Drawing.Point(134, 84);
            this.ModStartDateTB.Name = "ModStartDateTB";
            this.ModStartDateTB.ReadOnly = true;
            this.ModStartDateTB.Size = new System.Drawing.Size(81, 23);
            this.ModStartDateTB.TabIndex = 9;
            this.ModStartDateTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ModStartDateTB.MouseHover += new System.EventHandler(this.StartDateTB_MouseHover);
            // 
            // ModMaxPRovL
            // 
            this.ModMaxPRovL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModMaxPRovL.Location = new System.Drawing.Point(150, 19);
            this.ModMaxPRovL.Name = "ModMaxPRovL";
            this.ModMaxPRovL.Size = new System.Drawing.Size(65, 15);
            this.ModMaxPRovL.TabIndex = 8;
            this.ModMaxPRovL.Text = "Max";
            this.ModMaxPRovL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModMaxProvTB
            // 
            this.ModMaxProvTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.ModMaxProvTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ModMaxProvTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModMaxProvTB.Location = new System.Drawing.Point(150, 37);
            this.ModMaxProvTB.Name = "ModMaxProvTB";
            this.ModMaxProvTB.ReadOnly = true;
            this.ModMaxProvTB.Size = new System.Drawing.Size(65, 23);
            this.ModMaxProvTB.TabIndex = 7;
            this.ModMaxProvTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ModMaxProvTB.MouseHover += new System.EventHandler(this.MaxProvTB_MouseHover);
            // 
            // ModProvShownL
            // 
            this.ModProvShownL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModProvShownL.Location = new System.Drawing.Point(77, 19);
            this.ModProvShownL.Name = "ModProvShownL";
            this.ModProvShownL.Size = new System.Drawing.Size(67, 15);
            this.ModProvShownL.TabIndex = 6;
            this.ModProvShownL.Text = "Shown";
            this.ModProvShownL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModProvShownTB
            // 
            this.ModProvShownTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.ModProvShownTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ModProvShownTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModProvShownTB.Location = new System.Drawing.Point(77, 37);
            this.ModProvShownTB.Name = "ModProvShownTB";
            this.ModProvShownTB.ReadOnly = true;
            this.ModProvShownTB.Size = new System.Drawing.Size(67, 23);
            this.ModProvShownTB.TabIndex = 5;
            this.ModProvShownTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ModProvCountL
            // 
            this.ModProvCountL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModProvCountL.Location = new System.Drawing.Point(6, 19);
            this.ModProvCountL.Name = "ModProvCountL";
            this.ModProvCountL.Size = new System.Drawing.Size(65, 15);
            this.ModProvCountL.TabIndex = 4;
            this.ModProvCountL.Text = "Provinces";
            this.ModProvCountL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModProvCountTB
            // 
            this.ModProvCountTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.ModProvCountTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ModProvCountTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModProvCountTB.Location = new System.Drawing.Point(6, 37);
            this.ModProvCountTB.Name = "ModProvCountTB";
            this.ModProvCountTB.ReadOnly = true;
            this.ModProvCountTB.Size = new System.Drawing.Size(65, 23);
            this.ModProvCountTB.TabIndex = 3;
            this.ModProvCountTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ModProvCountTB.MouseHover += new System.EventHandler(this.ModProvCountTB_MouseHover);
            // 
            // ShowAllProvsMCB
            // 
            this.ShowAllProvsMCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ShowAllProvsMCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ShowAllProvsMCB.Image = global::EU4_PCP.Properties.Resources.UncheckedBox;
            this.ShowAllProvsMCB.Name = "ShowAllProvsMCB";
            this.ShowAllProvsMCB.Size = new System.Drawing.Size(180, 22);
            this.ShowAllProvsMCB.Text = "Show All Provinces";
            // 
            // GlobSetTSS1
            // 
            this.GlobSetTSS1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.GlobSetTSS1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.GlobSetTSS1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.GlobSetTSS1.Name = "GlobSetTSS1";
            this.GlobSetTSS1.Size = new System.Drawing.Size(177, 6);
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 542);
            this.Controls.Add(this.ModInfoGB);
            this.Controls.Add(this.GameInfoGB);
            this.Controls.Add(this.ModSelL);
            this.Controls.Add(this.ModSelCB);
            this.Controls.Add(this.ProvTableSB);
            this.Controls.Add(this.ProvTable);
            this.Controls.Add(this.ColorPickerGB);
            this.Controls.Add(this.MainMS);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMS;
            this.MaximizeBox = false;
            this.Name = "MainWin";
            this.Text = "EU4 Province Color Picker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
            this.Load += new System.EventHandler(this.MainWin_Load);
            this.MainMS.ResumeLayout(false);
            this.MainMS.PerformLayout();
            this.ColorPickerGB.ResumeLayout(false);
            this.ColorPickerGB.PerformLayout();
            this.GenCM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProvTable)).EndInit();
            this.ProvCM.ResumeLayout(false);
            this.GameInfoGB.ResumeLayout(false);
            this.GameInfoGB.PerformLayout();
            this.ModInfoGB.ResumeLayout(false);
            this.ModInfoGB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private DarkUI.Controls.DarkMenuStrip MainMS;
        private System.Windows.Forms.ToolStripMenuItem GlobSetM;
        private System.Windows.Forms.ToolStripMenuItem AutoLoadSM;
        private System.Windows.Forms.ToolStripMenuItem ModSetM;
        private System.Windows.Forms.ToolStripMenuItem DuplicatesSM;
        private System.Windows.Forms.ToolStripMenuItem StatsM;
        private System.Windows.Forms.ToolStripMenuItem ProvNamesSM;
        private System.Windows.Forms.ToolStripMenuItem LoadingTimeML;
        private System.Windows.Forms.ToolStripMenuItem LoadingValueML;
        private DarkUI.Controls.DarkGroupBox ColorPickerGB;
        internal System.Windows.Forms.DataGridView ProvTable;
        private DarkUI.Controls.DarkScrollBar ProvTableSB;
        private DarkUI.Controls.DarkComboBox ModSelCB;
        private DarkUI.Controls.DarkLabel ModSelL;
        private DarkUI.Controls.DarkGroupBox GameInfoGB;
        private DarkUI.Controls.DarkLabel GameBookmarkL;
        private DarkUI.Controls.DarkComboBox GameBookmarkCB;
        private DarkUI.Controls.DarkLabel GameStartDateL;
        private DarkUI.Controls.DarkTextBox GameStartDateTB;
        private DarkUI.Controls.DarkLabel GameMaxProvL;
        private DarkUI.Controls.DarkTextBox GameMaxProvTB;
        private DarkUI.Controls.DarkLabel GameProvShownL;
        private DarkUI.Controls.DarkTextBox GameProvShownTB;
        private DarkUI.Controls.DarkLabel GameProvCountL;
        private DarkUI.Controls.DarkTextBox GameProvCountTB;
        private DarkUI.Controls.DarkGroupBox ModInfoGB;
        private DarkUI.Controls.DarkLabel ModBookmarkL;
        private DarkUI.Controls.DarkComboBox ModBookmarkCB;
        private DarkUI.Controls.DarkLabel ModStartDateL;
        private DarkUI.Controls.DarkTextBox ModStartDateTB;
        private DarkUI.Controls.DarkLabel ModMaxPRovL;
        private DarkUI.Controls.DarkTextBox ModMaxProvTB;
        private DarkUI.Controls.DarkLabel ModProvShownL;
        private DarkUI.Controls.DarkTextBox ModProvShownTB;
        private DarkUI.Controls.DarkLabel ModProvCountL;
        private DarkUI.Controls.DarkTextBox ModProvCountTB;
        private DarkUI.Controls.DarkLabel GenColL;
        private DarkUI.Controls.DarkLabel NextProvNameL;
        private DarkUI.Controls.DarkTextBox NextProvNameTB;
        private DarkUI.Controls.DarkLabel BlueL;
        private DarkUI.Controls.DarkTextBox BlueTB;
        private DarkUI.Controls.DarkLabel GreenL;
        private DarkUI.Controls.DarkTextBox GreenTB;
        private DarkUI.Controls.DarkLabel RedL;
        private DarkUI.Controls.DarkTextBox RedTB;
        private DarkUI.Controls.DarkLabel NextProvNumberL;
        private DarkUI.Controls.DarkTextBox NextProvNumberTB;
        private System.Windows.Forms.FolderBrowserDialog BrowserFBD;
        private System.Windows.Forms.ToolTip TextBoxTT;
        private System.Windows.Forms.Label TempL;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvRed;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvGreen;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvBlue;
        public System.Windows.Forms.ToolStripMenuItem DisableLoadMCB;
        public System.Windows.Forms.ToolStripMenuItem RemLoadMCB;
        public System.Windows.Forms.ToolStripMenuItem FullyLoadMCB;
        public System.Windows.Forms.ToolStripMenuItem DefinNamesMCB;
        public System.Windows.Forms.ToolStripMenuItem LocNamesMCB;
        public System.Windows.Forms.ToolStripMenuItem DynNamesMCB;
        public System.Windows.Forms.ToolStripMenuItem CheckDupliMCB;
        public System.Windows.Forms.ToolStripMenuItem IgnoreRnwMCB;
        private DarkUI.Controls.DarkContextMenu ProvCM;
        private System.Windows.Forms.ToolStripMenuItem ChangeColorSM;
        private System.Windows.Forms.ToolStripMenuItem SelectInPickerMB;
        private DarkUI.Controls.DarkButton AddProvB;
        private System.Windows.Forms.ToolStripMenuItem DirectoriesM;
        private System.Windows.Forms.ToolStripMenuItem GamePathSM;
        private System.Windows.Forms.ToolStripTextBox GamePathMTB;
        private System.Windows.Forms.ToolStripMenuItem GamePathMB;
        private System.Windows.Forms.ToolStripMenuItem ModPathSM;
        private System.Windows.Forms.ToolStripTextBox ModPathMTB;
        private System.Windows.Forms.ToolStripMenuItem ModPathMB;
        private DarkUI.Controls.DarkContextMenu GenCM;
        private System.Windows.Forms.ToolStripMenuItem NewColorMB;
        private System.Windows.Forms.ToolStripSeparator GlobSetTSS1;
        public System.Windows.Forms.ToolStripMenuItem ShowAllProvsMCB;
    }
}

