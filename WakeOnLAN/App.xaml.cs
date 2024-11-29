﻿using WakeOnLAN.Repositories;

namespace WakeOnLAN
{
    public partial class App : Application
    {
        public static BaseRepository BaseRepo { get; private set; }


        public App(BaseRepository repo)
        {
            InitializeComponent();

            BaseRepo = repo;

            MainPage = new AppShell();
        }
    }
}
