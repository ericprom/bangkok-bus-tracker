using BUS.TRACKER.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.TRACKER.Model
{
   public class DataManager
    {
        static volatile DataManager _classInstance = null;
        static readonly object _threadSafetyLock = new object();
        public static DataManager shareInstance
        {
            get
            {
                if (_classInstance == null)
                {
                    lock (_threadSafetyLock)
                    {
                        if (_classInstance == null)
                            _classInstance = new DataManager();
                    }
                }
                return _classInstance;
            }
        }

        private RouteDataSource _RouteDataSource;
        public RouteDataSource RouteDataSource
        {
            get { return _RouteDataSource; }
            set
            {
                if (_RouteDataSource != value)
                {
                    _RouteDataSource = value;
                }
            }
        }
        private BtsDataSource _BtsDataSource;
        public BtsDataSource BtsDataSource
        {
            get { return _BtsDataSource; }
            set
            {
                if (_BtsDataSource != value)
                {
                    _BtsDataSource = value;
                }
            }
        }
        private MrtDataSource _MrtDataSource;
        public MrtDataSource MrtDataSource
        {
            get { return _MrtDataSource; }
            set
            {
                if (_MrtDataSource != value)
                {
                    _MrtDataSource = value;
                }
            }
        }
        private BrtDataSource _BrtDataSource;
        public BrtDataSource BrtDataSource
        {
            get { return _BrtDataSource; }
            set
            {
                if (_BrtDataSource != value)
                {
                    _BrtDataSource = value;
                }
            }
        }
        private ArlDataSource _ArlDataSource;
        public ArlDataSource ArlDataSource
        {
            get { return _ArlDataSource; }
            set
            {
                if (_ArlDataSource != value)
                {
                    _ArlDataSource = value;
                }
            }
        }
        private StopDataSource _StopDataSource;
        public StopDataSource StopDataSource
        {
            get { return _StopDataSource; }
            set
            {
                if (_StopDataSource != value)
                {
                    _StopDataSource = value;
                }
            }
        }
        private TransitDataSource _TransitDataSource;
        public TransitDataSource TransitDataSource
        {
            get { return _TransitDataSource; }
            set
            {
                if (_TransitDataSource != value)
                {
                    _TransitDataSource = value;
                }
            }
        }
        private TrackDataSource _TrackDataSource;
        public TrackDataSource TrackDataSource
        {
            get { return _TrackDataSource; }
            set
            {
                if (_TrackDataSource != value)
                {
                    _TrackDataSource = value;
                }
            }
        }
    }
}