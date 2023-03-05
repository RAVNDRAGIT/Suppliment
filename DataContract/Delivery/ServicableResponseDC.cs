using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Delivery
{
    public class ServicableResponseDC
    {
        public bool company_auto_shipment_insurance_setting { get; set; }
        public CovidZones covid_zones { get; set; }
        public string currency { get; set; }
        public Data data { get; set; }
        public int dg_courier { get; set; }
        public int eligible_for_insurance { get; set; }
        public bool insurace_opted_at_order_creation { get; set; }
        public bool is_allow_templatized_pricing { get; set; }
        public int is_latlong { get; set; }
        public bool is_old_zone_opted { get; set; }
        public bool is_zone_from_mongo { get; set; }
        public int label_generate_type { get; set; }
        public int on_new_zone { get; set; }
        public List<object> seller_address { get; set; }
        public int status { get; set; }
        public bool user_insurance_manadatory { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AvailableCourierCompany
    {
        public string air_max_weight { get; set; }
        public int assured_amount { get; set; }
        public object base_courier_id { get; set; }
        public string base_weight { get; set; }
        public int blocked { get; set; }
        public string call_before_delivery { get; set; }
        public int charge_weight { get; set; }
        public string city { get; set; }
        public int cod { get; set; }
        public double cod_charges { get; set; }
        public double cod_multiplier { get; set; }
        public string cost { get; set; }
        public int courier_company_id { get; set; }
        public string courier_name { get; set; }
        public string courier_type { get; set; }
        public int coverage_charges { get; set; }
        public string cutoff_time { get; set; }
        public string delivery_boy_contact { get; set; }
        public double delivery_performance { get; set; }
        public string description { get; set; }
        public string edd { get; set; }
        public int entry_tax { get; set; }
        public string estimated_delivery_days { get; set; }
        public string etd { get; set; }
        public int etd_hours { get; set; }
        public double freight_charge { get; set; }
        public int id { get; set; }
        public int is_custom_rate { get; set; }
        public bool is_hyperlocal { get; set; }
        public int is_international { get; set; }
        public bool is_rto_address_available { get; set; }
        public bool is_surface { get; set; }
        public int local_region { get; set; }
        public int metro { get; set; }
        //public double min_weight { get; set; }
        public int mode { get; set; }
        public bool odablock { get; set; }
        public int other_charges { get; set; }
        public string others { get; set; }
        public string pickup_availability { get; set; }
        public double pickup_performance { get; set; }
        public string pickup_priority { get; set; }
        public int pickup_supress_hours { get; set; }
        public string pod_available { get; set; }
        public string postcode { get; set; }
        public int qc_courier { get; set; }
        public string rank { get; set; }
        public double rate { get; set; }
        public double rating { get; set; }
        public string realtime_tracking { get; set; }
        public int region { get; set; }
        public double rto_charges { get; set; }
        public double rto_performance { get; set; }
        public int seconds_left_for_pickup { get; set; }
        public int ship_type { get; set; }
        public string state { get; set; }
        public string suppress_date { get; set; }
        public string suppress_text { get; set; }
        //public SuppressionDates suppression_dates { get; set; }
        public string surface_max_weight { get; set; }
        public double tracking_performance { get; set; }
        public object volumetric_max_weight { get; set; }
        public double weight_cases { get; set; }
    }

    public class CovidZones
    {
        public object delivery_zone { get; set; }
        public object pickup_zone { get; set; }
    }

    public class Data
    {
        public List<AvailableCourierCompany> available_courier_companies { get; set; }
        public object child_courier_id { get; set; }
        public int is_recommendation_enabled { get; set; }
        public int recommendation_advance_rule { get; set; }
        public RecommendedBy recommended_by { get; set; }
        public int recommended_courier_company_id { get; set; }
        public int shiprocket_recommended_courier_id { get; set; }
    }

    public class RecommendedBy
    {
        public int id { get; set; }
        public string title { get; set; }
    }

    public class Root
    {
       
    }

    public class SuppressionDates
    {
        public string action_on { get; set; }
        public string delay_remark { get; set; }
        public int delivery_delay_by { get; set; }
        public string delivery_delay_days { get; set; }
        public string delivery_delay_from { get; set; }
        public string delivery_delay_to { get; set; }
        public int pickup_delay_by { get; set; }
        public string pickup_delay_days { get; set; }
        public string pickup_delay_from { get; set; }
        public string pickup_delay_to { get; set; }
    }






}
