using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day20.Common
{
    internal class Output(string id) : IModule
    {
        public string Id { get; init; } = id;

        public void ConnectReceiver(string id)
        {

        }

        public void Process(Pulse p)
        {

        }
    }
}
