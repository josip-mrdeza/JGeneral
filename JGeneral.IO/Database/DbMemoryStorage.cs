using System;
using System.Collections;
using System.Collections.Generic;

namespace JGeneral.IO.Database
{
    public class DbMemoryStorage
    {
        private Dictionary<string, object> _storage;

        public Dictionary<string, object> Storage
        {
            get
            {
                return _storage;
            }
            set
            {
                _storage = value;
            }
        }

        private object dicLock = new object();
        public DbMemoryStorage()
        {
            Storage = new Dictionary<string, object>();
        }
    }
}