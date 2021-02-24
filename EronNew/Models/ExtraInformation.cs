using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EronNew.Models
{
    [Table("PostsExtraInformation")]
    public partial class ExtraInformation
    {
        public long PostId { get; set; }
        public long? FPostId { get; set; }
        public bool AirCondition { get; set; }
        public bool Bbq { get; set; }
        public bool Elevator { get; set; }
        public bool Fireplace { get; set; }
        public bool Garden { get; set; }
        public int? GardenSpace { get; set; }
        public bool Gym { get; set; }
        public bool Hall { get; set; }
        public bool Heating { get; set; }
        public string HeatingSystem { get; set; }
        public bool Kitchen { get; set; }
        public bool Livingroom { get; set; }
        public bool Maidroom { get; set; }
        public bool Master { get; set; }
        public bool RoofFloor { get; set; }
        public bool SemiOutdoor { get; set; }
        public int? SemiOutdoorSquare { get; set; }
        public bool Storageroom { get; set; }
        public int? StorageroomSquare { get; set; }
        public bool Swimmingpool { get; set; }
        public bool Wc { get; set; }
        public bool SecureDoor { get; set; }
        public bool AluminumFrames { get; set; }
        public bool HomeAlarm { get; set; }
        public bool FTTH { get; set; }
        public bool HouseKeepingMoney { get; set; }
        public bool NewConstruction { get; set; }
        public bool Offer { get; set; }
        public bool Investment { get; set; }
        public bool BrokerFee { get; set; }

        public virtual PostsModel FPost { get; set; }
    }
}
