﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace furMix
{
    public partial class Main : Form
    {
        string filepath;
        bool full, playing, playing1, loop, mute;
        int type = 1;
        Play1 pl1 = new Play1();
        Play2 pl2 = new Play2();
        Background back = new Background();
        Bitmap bmp;
        Graphics gr;
        Color color;
        List<string> filepath1 = new List<string>();
        List<int> type1 = new List<int>();
        List<Color> color1 = new List<Color>();
        public Main()
        {
            InitializeComponent();
            pl1.Opacity = 0;
            Preview.settings.mute = true;
            pl1.Video.settings.mute = true;
            pl2.Video.settings.mute = true;
            VerTxt.Text = "furMix 2020. Build " + Properties.Settings.Default.Version + ". Beta 1.\n For testing purposes only.";
        }

        private void ShowFurry()
        {
            try
            {
                Screen[] sc;
                sc = Screen.AllScreens;
                if (sc.Length > 1)
                {
                    back.Left = sc[1].Bounds.Width;
                    back.Top = sc[1].Bounds.Height;
                    back.Location = sc[1].Bounds.Location;
                    back.WindowState = FormWindowState.Maximized;
                    back.Show();
                    pl1.Left = sc[1].Bounds.Width;
                    pl1.Top = sc[1].Bounds.Height;
                    pl1.Location = sc[1].Bounds.Location;
                    pl1.WindowState = FormWindowState.Maximized;
                    pl1.Show();
                    pl2.Left = sc[1].Bounds.Width;
                    pl2.Top = sc[1].Bounds.Height;
                    pl2.Location = sc[1].Bounds.Location;
                    pl2.WindowState = FormWindowState.Maximized;
                    pl2.Show();
                }
                else
                {
                    throw new Exception("Only 1 monitor was found. To use furMix, you need at least 2 monitors");
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Preview.Ctlcontrols.stop();
                pl1.Opacity = 1;
                pl2.Opacity = 0;
                pl2.Video.close();
                if (type == 0)
                {
                    pl1.Opacity = 0;
                    pl1.Video.close();
                    back.BackColor = color;
                }
                else if (type == 1 || type == 2)
                {
                    pl1.Video.URL = filepath;
                    pl1.Video.Visible = true;
                    pl1.Video.Ctlcontrols.play();
                    playing = false;
                    button4_Click(sender, e);
                }
                if (type == 2)
                {
                    loop = false;
                    loopbtn_Click(sender, e);
                }
                if (type == 1)
                {
                    playbtn.Visible = true;
                    timelineShow.Visible = true;
                    timeShow.Visible = true;
                    timelineShow.Maximum = Convert.ToInt32(pl1.Video.currentMedia.durationString.Substring(0, 2)) * 60 + Convert.ToInt32(pl1.Video.currentMedia.durationString.Substring(3, 2));
                    showTimer.Enabled = true;
                }
                else
                {
                    playbtn.Visible = false;
                    timelineShow.Visible = false;
                    timeShow.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (full)
                {
                    HideFurry();
                    full = false;
                    timer1.Enabled = false;
                    fullbtn.BackColor = Color.FromArgb(100, 100, 100);
                }
                else
                {
                    timer1.Enabled = true;
                    ShowFurry();
                    full = true;
                    fullbtn.BackColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Screen[] sc = Screen.AllScreens;
                if (sc.Length > 1)
                {
                    gr = CreateGraphics();
                    bmp = new Bitmap(sc[1].Bounds.Width, sc[1].Bounds.Height, gr);
                    gr = Graphics.FromImage(bmp);
                    gr.CopyFromScreen(sc[1].Bounds.X, sc[1].Bounds.Y, 0, 0, new Size(sc[1].Bounds.Width, sc[1].Bounds.Height));
                    picture.Image = bmp;
                    //gr.Dispose();
                    //bmp.Dispose();
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Preview.Ctlcontrols.stop();
                pl1.Video.Ctlcontrols.stop();
                pl2.Video.Ctlcontrols.stop();
                Application.Exit();
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (playing)
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.Ctlcontrols.pause();
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.Ctlcontrols.play();
                    }
                    playbtn.Text = "Pause";
                    playing = false;
                }
                else
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.Ctlcontrols.play();
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.Ctlcontrols.pause();
                    }
                    playbtn.Text = "Play";
                    playing = true;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("This program has been made as alternative to vMix. \n Made by WinWins and odinokoe_4m0 \n Design: WinWins and odinokoe_4m0 \n Judge: Misha Ter \n \n Licensed to " + Properties.Settings.Default.Name + "\n \n Version " + Properties.Settings.Default.Version + " Beta 1 \n (C) 2020 DaniMat Corp.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (mute)
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.settings.mute = false;
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.settings.mute = false;
                    }
                    mute = false;
                    mutebtn.BackColor = Color.FromArgb(100, 100, 100);
                }
                else
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.settings.mute = true;
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.settings.mute = true;
                    }
                    mute = true;
                    mutebtn.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void loopbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (loop)
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.settings.setMode("loop", false);
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.settings.setMode("loop", false);
                    }
                    loop = false;
                    loopbtn.BackColor = Color.FromArgb(100, 100, 100);
                }
                else
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.settings.setMode("loop", true);
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.settings.setMode("loop", true);
                    }
                    loop = true;
                    loopbtn.BackColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    return;
                }
                Preview.Ctlcontrols.stop();
                type = type1[listView1.SelectedItems[0].Index];
                if (type != 0)
                {
                    filepath = filepath1[listView1.SelectedItems[0].Index];
                    Preview.Visible = true;
                    Preview.URL = filepath;
                    if (type == 2)
                    {
                        Preview.settings.setMode("loop", true);
                    }
                    else
                    {
                        Preview.settings.setMode("loop", false);
                    }
                }
                else
                {
                    color = color1[listView1.SelectedItems[0].Index];
                    Preview.Visible = false;
                    Preview.Ctlcontrols.stop();
                    pictureBox1.BackColor = color;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void previewTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                timelinePrev.Value = Convert.ToInt32(Preview.Ctlcontrols.currentPosition);
                PreviewTime.Text = Preview.Ctlcontrols.currentPositionString + "/" + Preview.currentMedia.durationString;
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void Preview_OpenStateChange(object sender, AxWMPLib._WMPOCXEvents_OpenStateChangeEvent e)
        {
            try
            {
                if (Preview.openState == WMPLib.WMPOpenState.wmposMediaOpen)
                {
                    Preview.settings.mute = true;
                    if (type == 2)
                    {
                        PreviewTime.Visible = false;
                        timelinePrev.Visible = false;
                        playbtnprev.Visible = false;
                    }
                    else
                    {
                        timelinePrev.Visible = true;
                        PreviewTime.Visible = true;
                        playbtnprev.Visible = true;
                        int seconds = Convert.ToInt32(Preview.currentMedia.durationString.Substring(0, 2)) * 60 + Convert.ToInt32(Preview.currentMedia.durationString.Substring(3, 2));
                        timelinePrev.Maximum = seconds;
                        Console.WriteLine(seconds);
                        previewTimer.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void timelinePrev_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                previewTimer.Enabled = false;
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void timelinePrev_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Preview.Ctlcontrols.pause();
                Preview.Ctlcontrols.currentPosition = timelinePrev.Value;
                if (playing1)
                {
                    Preview.Ctlcontrols.play();
                    previewTimer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void showTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (full)
                {
                    if (pl1.Opacity == 1)
                    {
                        timeShow.Text = pl1.Video.Ctlcontrols.currentPositionString + "/" + pl1.Video.currentMedia.durationString;
                        timelineShow.Value = (int)pl1.Video.Ctlcontrols.currentPosition;
                    }
                    else if (pl2.Opacity == 1)
                    {
                        timeShow.Text = pl2.Video.Ctlcontrols.currentPositionString + "/" + pl2.Video.currentMedia.durationString;
                        timelineShow.Value = (int)pl2.Video.Ctlcontrols.currentPosition;
                    }
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void timelineShow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                showTimer.Enabled = false;
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void timelineShow_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (full)
                {
                    if (pl1.Opacity == 1)
                    {
                        pl1.Video.Ctlcontrols.currentPosition = timelineShow.Value;
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl2.Video.Ctlcontrols.currentPosition = timelineShow.Value;
                    }
                    if (playing)
                    {
                        showTimer.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!playing1)
                {
                    Preview.Ctlcontrols.play();
                    playbtnprev.Text = "Pause";
                    playing1 = true;
                }
                else
                {
                    Preview.Ctlcontrols.pause();
                    playbtnprev.Text = "Play";
                    playing1 = false;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Preview.Ctlcontrols.stop();
                if (type == 0)
                {
                    if (pl1.Opacity == 1)
                    {
                        pl2.BackColor = color;
                        pl2.Video.Visible = false;
                        double opacity1 = 1;
                        double opacity2 = 0;
                        while (opacity1 >= 0)
                        {
                            pl1.Opacity = opacity1;
                            pl2.Opacity = opacity2;
                            Thread.Sleep(100);
                            opacity1 -= 0.05;
                            opacity2 += 0.05;
                        }
                        pl1.Video.close();
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl1.BackColor = color;
                        pl1.Video.Visible = false;
                        double opacity1 = 0;
                        double opacity2 = 1;
                        while (opacity2 >= 0)
                        {
                            pl1.Opacity = opacity1;
                            pl2.Opacity = opacity2;
                            Thread.Sleep(100);
                            opacity1 += 0.05;
                            opacity2 -= 0.05;
                        }
                        pl2.Video.close();
                    }
                }
                else if (type == 1 || type == 2)
                {
                    if (pl1.Opacity == 1)
                    {
                        pl2.Opacity = 0.05;
                        pl2.Video.URL = filepath;
                        pl2.Video.Visible = true;
                        pl2.Video.Ctlcontrols.play();
                        double opacity1 = 1;
                        double opacity2 = 0.05;
                        while (opacity1 > 0)
                        {
                            pl1.Opacity = opacity1;
                            pl2.Opacity = opacity2;
                            Thread.Sleep(100);
                            opacity1 -= 0.05;
                            opacity2 += 0.05;
                        }
                        pl1.Opacity = 0;
                        pl1.Video.close();
                        pl2.Opacity = 1;
                    }
                    else if (pl2.Opacity == 1)
                    {
                        pl1.Opacity = 0.05;
                        pl1.Video.URL = filepath;
                        pl2.Video.Visible = true;
                        pl1.Video.Ctlcontrols.play();
                        double opacity1 = 0.05;
                        double opacity2 = 1;
                        while (opacity2 > 0)
                        {
                            pl1.Opacity = opacity1;
                            pl2.Opacity = opacity2;
                            Thread.Sleep(100);
                            opacity1 += 0.05;
                            opacity2 -= 0.05;
                        }
                        pl1.Opacity = 1;
                        pl2.Opacity = 0;
                        pl2.Video.close();
                    }
                    if (type == 2)
                    {
                        loop = false;
                        loopbtn_Click(sender, e);
                    }
                    playing = false;
                    button4_Click(sender, e);
                }
                if (type == 1)
                {
                    playbtn.Visible = true;
                }
                else
                {
                    playbtn.Visible = false;
                }
                if (type == 1)
                {
                    if (pl1.Opacity == 1)
                    {
                        playbtn.Visible = true;
                        timelineShow.Visible = true;
                        timeShow.Visible = true;
                        timelineShow.Maximum = Convert.ToInt32(pl1.Video.currentMedia.durationString.Substring(0, 2)) * 60 + Convert.ToInt32(pl1.Video.currentMedia.durationString.Substring(3, 2));
                        showTimer.Enabled = true;
                    }
                    else if (pl2.Opacity == 1)
                    {
                        playbtn.Visible = true;
                        timelineShow.Visible = true;
                        timeShow.Visible = true;
                        timelineShow.Maximum = Convert.ToInt32(pl2.Video.currentMedia.durationString.Substring(0, 2)) * 60 + Convert.ToInt32(pl2.Video.currentMedia.durationString.Substring(3, 2));
                        showTimer.Enabled = true;
                    }
                }
                else
                {
                    playbtn.Visible = false;
                    timelineShow.Visible = false;
                    timeShow.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                AddInput addInput = new AddInput();
                addInput.ShowDialog();
                if (!AddInput.canceled)
                {
                    type = AddInput.type;
                    if (AddInput.filepath == "" && type > 0)
                    {
                        return;
                    }
                    type1.Add(type);
                    if (type == 0)
                    {
                        filepath1.Add("");
                        color = AddInput.color;
                        color1.Add(color);
                        listView1.Items.Add("Color \n" + color.Name);
                    }
                    else
                    {
                        filepath1.Add(AddInput.filepath);
                        color1.Add(Color.Black);
                        if (type == 1)
                        {
                            listView1.Items.Add("Video \n" + AddInput.filepath, listView1.Items.Count);
                        }
                        else if (type == 2)
                        {
                            listView1.Items.Add("Photo \n" + AddInput.filepath);
                        }
                    }
                }
                //throw new NotImplementedException("Еще не готово, хули жмешь");
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }

        private void HideFurry()
        {
            try
            {
                pl1.Hide();
                pl2.Hide();
                back.Hide();
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ShowError(ex);
                error.ShowDialog();
            }
        }
    }
}