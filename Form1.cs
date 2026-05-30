using System;
using System.Management;
using System.Media;
using System.Windows.Forms;

namespace DeviceEventSoundPlayer {
    public partial class Form1 : Form {
        private readonly SoundPlayer connectSound = new SoundPlayer(@"C:\Windows\Media\Windows Hardware Insert.wav");
        private readonly SoundPlayer disconnectSound = new SoundPlayer(@"C:\Windows\Media\Windows Hardware Remove.wav");

        private ManagementEventWatcher insertWatcher;
        private ManagementEventWatcher removeWatcher;

        public Form1() {
            InitializeComponent();

            // ui state
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
            Visible = false;

            StartWatchers();
        }

        private void StartWatchers() {
            insertWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2"));
            insertWatcher.EventArrived += (s, e) => {
                connectSound.Play();
            };

            insertWatcher.Start();

            removeWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3"));
            removeWatcher.EventArrived += (s, e) => {
                disconnectSound.Play();
            };

            removeWatcher.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            insertWatcher?.Stop();
            removeWatcher?.Stop();

            base.OnFormClosing(e);
        }
    }
}
