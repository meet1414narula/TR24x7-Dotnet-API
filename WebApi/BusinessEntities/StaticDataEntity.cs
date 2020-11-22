using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class StaticDataEntity
    {
        public List<string> VehicleLengths { get; set; }

        public List<RoadLineEntity> RoadLines { get; set; }

        public List<EnquiryStatusEntity> EnquiryStatus { get; set; }

        public List<CreatedDateEntity> CreatedDateFilter { get; set; }

        public List<MovingDateEntity> MovingDateFilter { get; set; }

        public List<int> MaxWeights { get; set; }
        public List<CityEntity> Cities { get; set; }
        public List<CityEntity> States { get; set; }
        public List<VehicleTypeEntity> VehicleTypes { get; set; }
        public List<UserTypeEntity> UserTypes { get; set; }

        public List<UserDetailsEntity> UserDetails { get; set; }
        public List<UserProfessionEntity> UserProfessions { get; set; }
        public List<VehicleCompanyEntity> VehicleCompanies { get; set; }
        public List<MaterialTypeEntity> MaterialTypes { get; set; }
        public string SupportNumber { get; set; }
    }

    public class VehicleTypeEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreatedDateEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class MovingDateEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class RoadLineEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EnquiryStatusEntity
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

    public class UserTypeEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class UserDetailsEntity
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
    }

    public class UserProfessionEntity
    {
        public int Id { get; set; }
        public string Profession { get; set; }
    }

    public class VehicleCompanyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MaterialTypeEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public int DisplayOrder { get; set; }
    }

    public class CityEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ActualName { get; set; }
        public string State { get; set; }
        public bool? IsActive { get; set; }
    }
}
