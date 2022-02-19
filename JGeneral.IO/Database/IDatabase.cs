using System;
using System.Collections.Generic;

namespace JGeneral.IO.Database
{
    public interface IDatabase
    {
        public JWorker[] DbWorkers { get; }
        public List<string> PastEvents { get; }

        public void AssignWork(Action work, string workId);
        public void AssignWork(Action work);

        public void EstablishPipe();

    }
}