namespace Tarsvin.Runner.EventListerns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tarsvin.Runner;

    public class EventListners<T>
    {
        private ParallelRunner runner;

        public void AttachInvokeCmptEvent(ParallelRunner runner)
        {
            this.runner = runner;
            this.runner.InvokeCmpt += new EventHandler(InvokeCmpt);
        }

        private void InvokeCmpt(object sender, EventArgs e)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\nMethod Invoked \n\n\n\n\n\n");
        }

        private void TypeCmpt()
        {

        }

        private void DllCompt()
        {

        }
    }
}
