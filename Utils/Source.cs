using NetworkSourceSimulator;

namespace flyingApp
{
    class NetworkDataLoader : ISubject
    {
        private List<IObserver> _obs;

        public NetworkDataLoader()
        {
            _obs = new List<IObserver>();
        }

        public void DownloadData(List<IBaseObject> lis)
        {
            NetworkSourceSimulator.NetworkSourceSimulator net =
                new NetworkSourceSimulator.NetworkSourceSimulator("example_data.ftr.txt", 2, 3);
            Thread netThread = new Thread(() =>
            {
                net.Run();
            });
            foreach (var obj in lis)
            {
                this.attachListener(obj as IObserver);
            }
            net.OnNewDataReady += (sender, data) =>
            {
                Message mes = net.GetMessageAt(data.MessageIndex);
                IBaseObject temp = ByteDecoder.ByteMessageDecode(mes.MessageBytes);
                lis.Add(temp);
                this.attachListener(temp as IObserver);
            };
            netThread.Start();
        }

        public void UpdateData(List<IBaseObject> lis)
        {
            NetworkSourceSimulator.NetworkSourceSimulator net =
                new NetworkSourceSimulator.NetworkSourceSimulator("example.ftre", 100, 500);
            foreach (var obj in lis)
            {
                this.attachListener(obj as IObserver);
            }
            Thread netThread = new Thread(net.Run);
            net.OnIDUpdate += (sender, data) =>
            {
                foreach (var o in _obs)
                {
                    if (((IBaseObject)o).ObjectID == data.ObjectID)
                    {
                        o.Update(data);
                    }
                }
            };
            net.OnPositionUpdate += (sender, data) =>
            {
                foreach (var o in _obs)
                {
                    if (((IBaseObject)o).ObjectID == data.ObjectID)
                    {
                        o.Update(data);
                    }
                }
            };
            net.OnContactInfoUpdate += (sender, data) =>
            {
                foreach (var o in _obs)
                {
                    if (((IBaseObject)o).ObjectID == data.ObjectID)
                    {
                        o.Update(data);
                    }
                }
            };
            netThread.Start();
        }

        public void attachListener(IObserver o)
        {
            _obs.Add(o);
        }

        public void detachListener(IObserver o)
        {
            _obs.Remove(o);
        }
    }
}
