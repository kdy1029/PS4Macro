﻿// PS4Macro (File: PS4Macro.cs)
//
// Copyright (c) 2017 Komefai
//
// Visit http://komefai.com for more information
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using PS4RemotePlayInterceptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro
{
    public partial class PS4Macro : Form
    {
        private MacroPlayer m_MacroPlayer;
        private SaveLoadHelper m_SaveLoadHelper;

        /* Constructor */
        public PS4Macro()
        {
            InitializeComponent();

            // Create macro player
            m_MacroPlayer = new MacroPlayer();
            m_MacroPlayer.PropertyChanged += MacroPlayer_PropertyChanged;

            // Create save/load helper
            m_SaveLoadHelper = new SaveLoadHelper(m_MacroPlayer);

            // Inject into PS4 Remote Play
            Interceptor.Callback = new InterceptionDelegate(m_MacroPlayer.OnReceiveData);
            Interceptor.Inject();
        }

        private void MacroPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsPlaying":
                    {
                        playButton.ForeColor = m_MacroPlayer.IsPlaying ? Color.Green : DefaultForeColor;
                        break;
                    }

                case "IsRecording":
                    {
                        recordButton.ForeColor = m_MacroPlayer.IsRecording ? Color.Red : DefaultForeColor;
                        break;
                    }

                case "CurrentTick":
                    {
                        BeginInvoke((MethodInvoker)delegate
                        {
                            currentTickToolStripStatusLabel.Text = m_MacroPlayer.CurrentTick.ToString();
                        });
                        break;
                    }

                case "Sequence":
                    {
                        break;
                    }
            }
        }

        /* Playback buttons methods */
        #region Playback Buttons

        private void playButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Play();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Pause();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Stop();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Record();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Clear();
        }
        #endregion

        /* Menu strip methods */
        #region Menu Strip

        #region File
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Clear();
            m_SaveLoadHelper.ClearCurrentFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SaveLoadHelper.Load();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SaveLoadHelper.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SaveLoadHelper.SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Playback
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Play();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Pause();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Stop();
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Record();
        }
        #endregion

        #endregion

        /* Status strip methods */
        #region Status Strip
        private void urlToolStripStatusLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://komefai.com");
        }
        #endregion
    }
}
