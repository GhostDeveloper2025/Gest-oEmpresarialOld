using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wrapper.WhatsGW
{
    internal class Mensagem
    {
        //public string apikey { get; set; } = "51694936-64b2-46cc-b9d6-20c56913cdc7";

        public string apikey { get; set; } = "4217937c-bc9f-4ca8-ab58-ddae4f65e880";
        //public string phone_number { get; set; } = "5562998518719";

        public string phone_number { get; set; } = "5562992558598";

        public string contact_phone_number { get; set; }
        public string message_type { get; set; } = "text";
        public string message_body { get; set; }
        public string message_body_filename { get; set; }
    }
}
