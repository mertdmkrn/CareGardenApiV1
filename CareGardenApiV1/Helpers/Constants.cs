using CareGardenApiV1.Model;

namespace CareGardenApiV1.Helpers
{
    public static class Constants
    {
        public static class ImageSizes
        {
            public static string BusinessListImageSize = "300x212";
        }

        public static List<string> Cities = new List<string>
        {
            "Adana",
            "Adıyaman",
            "Afyon",
            "Ağrı",
            "Amasya",
            "Ankara",
            "Antalya",
            "Artvin",
            "Aydın",
            "Balıkesir",
            "Bilecik",
            "Bingöl",
            "Bitlis",
            "Bolu",
            "Burdur",
            "Bursa",
            "Çanakkale",
            "Çankırı",
            "Çorum",
            "Denizli",
            "Diyarbakır",
            "Edirne",
            "Elazığ",
            "Erzincan",
            "Erzurum",
            "Eskişehir",
            "Gaziantep",
            "Giresun",
            "Gümüşhane",
            "Hakkari",
            "Hatay",
            "Isparta",
            "Mersin",
            "İstanbul",
            "İzmir",
            "Kars",
            "Kastamonu",
            "Kayseri",
            "Kırklareli",
            "Kırşehir",
            "Kocaeli",
            "Konya",
            "Kütahya",
            "Malatya",
            "Manisa",
            "Kahramanmaraş",
            "Mardin",
            "Muğla",
            "Muş",
            "Nevşehir",
            "Niğde",
            "Ordu",
            "Rize",
            "Sakarya",
            "Samsun",
            "Siirt",
            "Sinop",
            "Sivas",
            "Tekirdağ",
            "Tokat",
            "Trabzon",
            "Tunceli",
            "Şanlıurfa",
            "Uşak",
            "Van",
            "Yozgat",
            "Zonguldak",
            "Aksaray",
            "Bayburt",
            "Karaman",
            "Kırıkkale",
            "Batman",
            "Şırnak",
            "Bartın",
            "Ardahan",
            "Iğdır",
            "Yalova",
            "Karabük",
            "Kilis",
            "Osmaniye",
            "Düzce"
        };

        public static List<OfficialDay> OfficialDays = new List<OfficialDay>
        {
            new OfficialDay("Demokrasi ve Millî Birlik Günü", "Democracy and National Unity Day", "Cumartesi", new DateTime(2023, 07, 15)),
            new OfficialDay("Zafer Bayramı", "Victory Day", "Çarşamba", new DateTime(2023, 08, 30)),
            new OfficialDay("Cumhuriyet Bayramı", "Republic Day", "Pazar", new DateTime(2023, 10, 29)),
            new OfficialDay("Yılbaşı", "New Year's Day", "Pazartesi", new DateTime(2024, 01, 01)),
            new OfficialDay("Ramazan Bayramı (1. Gün)", "Ramadan Feast (1st Day)", "Çarşamba", new DateTime(2024, 04, 10)),
            new OfficialDay("Ramazan Bayramı (2. Gün)", "Ramadan Feast (2nd Day)", "Perşembe", new DateTime(2024, 04, 11)),
            new OfficialDay("Ramazan Bayramı (3. Gün)", "Ramadan Feast (3rd Day)", "Cuma", new DateTime(2024, 04, 12)),
            new OfficialDay("Ulusal Egemenlik ve Çocuk Bayramı", "National Sovereignty and Children's Day", "Salı", new DateTime(2024, 04, 23)),
            new OfficialDay("Emek ve Dayanışma Günü", "Labor and Solidarity Day", "Çarşamba", new DateTime(2024, 05, 01)),
            new OfficialDay("Atatürk'ü Anma, Gençlik ve Spor Bayramı", "Commemoration of Atatürk, Youth and Sports Day", "Pazar", new DateTime(2024, 05, 19)),
            new OfficialDay("Kurban Bayramı (1. Gün)", "Eid al-Adha (1st Day)", "Pazar", new DateTime(2024, 06, 16)),
            new OfficialDay("Kurban Bayramı (2. Gün)", "Eid al-Adha (2nd Day)", "Pazartesi", new DateTime(2024, 06, 17)),
            new OfficialDay("Kurban Bayramı (3. Gün)", "Eid al-Adha (3rd Day)", "Salı", new DateTime(2024, 06, 18)),
            new OfficialDay("Kurban Bayramı (4 Gün)", "Eid al-Adha (4th Day)", "Çarşamba", new DateTime(2024, 06, 19)),
            new OfficialDay("Demokrasi ve Millî Birlik Günü", "Democracy and National Unity Day", "Pazartesi", new DateTime(2024, 07, 15)),
            new OfficialDay("Zafer Bayramı", "Victory Day", "Cuma", new DateTime(2024, 08, 30)),
            new OfficialDay("Cumhuriyet Bayramı", "Republic Day", "Salı", new DateTime(2024, 10, 29)),
            new OfficialDay("Yılbaşı", "New Year's Day", "Çarşamba", new DateTime(2025, 01, 01)),
            new OfficialDay("Ramazan Bayramı (1. Gün)", "Ramadan Feast (1st Day)", "Pazar", new DateTime(2025, 03, 30)),
            new OfficialDay("Ramazan Bayramı (2. Gün)", "Ramadan Feast (2nd Day)", "Pazartesi", new DateTime(2025, 03, 31)),
            new OfficialDay("Ramazan Bayramı (3. Gün)", "Ramadan Feast (3rd Day)", "Salı", new DateTime(2025, 04, 01)),
            new OfficialDay("Ulusal Egemenlik ve Çocuk Bayramı", "National Sovereignty and Children's Day", "Çarşamba", new DateTime(2025, 04, 23)),
            new OfficialDay("Emek ve Dayanışma Günü", "Labor and Solidarity Day", "Perşembe", new DateTime(2025, 05, 01)),
            new OfficialDay("Atatürk'ü Anma, Gençlik ve Spor Bayramı", "Commemoration of Atatürk, Youth and Sports Day", "Pazartesi", new DateTime(2025, 05, 19)),
            new OfficialDay("Kurban Bayramı (1. Gün)", "Eid al-Adha (1st Day)", "Cuma", new DateTime(2025, 06, 06)),
            new OfficialDay("Kurban Bayramı (2. Gün)", "Eid al-Adha (2nd Day)", "Cumartesi", new DateTime(2025, 06, 07)),
            new OfficialDay("Kurban Bayramı (3. Gün)", "Eid al-Adha (3rd Day)", "Pazar", new DateTime(2025, 06, 08)),
            new OfficialDay("Kurban Bayramı (4 Gün)", "Eid al-Adha (4th Day)", "Pazartesi", new DateTime(2025, 06, 09)),
            new OfficialDay("Demokrasi ve Millî Birlik Günü", "Democracy and National Unity Day", "Salı", new DateTime(2025, 07, 15)),
            new OfficialDay("Zafer Bayramı", "Victory Day", "Cumartesi", new DateTime(2025, 08, 30)),
            new OfficialDay("Cumhuriyet Bayramı", "Republic Day", "Çarşamba", new DateTime(2025, 10, 29))
        };

        public static List<string> FaqEnCategories = new List<string>
        {
            "Register",
            "Appointment",
            "Cancellation",
            "Reminder",
            "Payments",
            "Review",
            "Account",
            "Venues",
            "Others"
        };

        public static List<string> FaqCategories = new List<string>
        {
            "Kayıt",
            "Randevu",
            "İptal",
            "Hatırlatma",
            "Ödemeler",
            "İnceleme",
            "Hesap",
            "Mekanlar",
            "Diğer"
        };

        public static double DistanceValue = 160.9344;
    }
}
