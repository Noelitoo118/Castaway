namespace Castaway
{
    /// <summary>
    /// Kész szövegek a játékhoz. Szabadon használhatod, bővítheted és átírhatod őket.
    /// Véletlen elem egy tömbből: Texts.GatherTexts[random.Next(Texts.GatherTexts.Length)]
    /// </summary>
    internal static class Texts
    {
        public static readonly string Title = @"
  ____          _
 / ___|__ _ ___| |_ __ ___      ____ _ _   _
| |   / _` / __| __/ _` \ \ /\ / / _` | | | |
| |__| (_| \__ \ || (_| |\ V  V / (_| | |_| |
 \____\__,_|___/\__\__,_| \_/\_/ \__,_|\__, |
                                       |___/";

        public static readonly string Intro =
            "A hajód elsüllyedt a viharban. Egy ismeretlen szigeten térsz magadhoz,\n" +
            "csak a ruha van rajtad. Ha jelzőtüzet építesz, egy arra járó hajó észrevehet.";

        // ---- Akciók (K3) ----

        public static readonly string[] GatherTexts =
        {
            "Bogyókat szedtél a bozótosban.",
            "Kókuszdiót ráztál le egy pálmafáról.",
            "Kagylókat gyűjtöttél a sziklák között.",
            "Ehető gyökereket ástál ki a homokból.",
        };

        public static readonly string[] WaterTexts =
        {
            "Esővizet gyűjtöttél egy nagy pálmalevélben.",
            "Egy sziklamélyedésben esővíz gyűlt össze.",
            "Kókuszdiók levét gyűjtötted össze.",
            "Egy vékony erecskét találtál a fák között.",
        };

        public static readonly string[] MaterialTexts =
        {
            "Végigjártad a partot, és összeszedtél mindent, ami hasznos lehet.",
            "Uszadékfát, köveket és pálmaleveleket hordtál a táborba.",
            "Az erdőszélen ágakat törtél, a patakmederben köveket válogattál.",
            "Egy éles kagylóval leveleket vágtál, közben fát és követ is gyűjtöttél.",
        };

        public static readonly string[] RestTexts =
        {
            "Lefeküdtél egy pálmafa árnyékába.",
            "Szundítottál egyet a meleg homokon.",
            "Csak ültél, nézted a tengert, és összeszedted magad.",
        };

        // ---- Éjszaka (K5) ----

        public static readonly string[] QuietNights =
        {
            "Csendes éjszaka volt, csak a hullámokat hallottad.",
            "Tücskök ciripeltek egész éjjel.",
            "A csillagos ég alatt aludtál el.",
        };

        public static readonly string[] WindTexts =
        {
            "Éjjel feltámadt a szél, és elfújta a fa egy részét.",
            "Egy erős széllökés a tengerbe sodorta a fa egy részét.",
        };

        public static readonly string[] AnimalTexts =
        {
            "Éjjel egy vaddisznó feltúrta a készleteidet.",
            "Majmok lopták el az élelmed egy részét, amíg aludtál.",
        };

        public static readonly string[] ShipMissed =
        {
            "A távolban elhaladt egy hajó, de nem vett észre.",
            "Egy hajó füstje tűnt fel a láthatáron, aztán eltűnt.",
        };

        // ---- Halászat (A1) ----

        public static readonly string[] FishingSuccess =
        {
            "Egy kövér hal akadt a horgodra!",
            "A sekély vízben sikerült elkapnod egy halat.",
        };

        public static readonly string[] FishingFail =
        {
            "Egész délután vártál, de semmi sem harapott.",
            "A hal az utolsó pillanatban kicsúszott a kezedből.",
        };

        // ---- Felfedezés (A4) ----

        public static readonly string[] LocationTexts =
        {
            "Forrás: Kristálytiszta víz bugyog elő a sziklák közül.",
            "Banánliget: A sziget belsejében roskadoznak a fák a banántól.",
            "Kőfejtő: Lapos, könnyen hasítható kövek borítják a domboldalt.",
            "Elhagyott tábor: Valaki már járt itt előtted, és hagyott hátra pár dolgot.",
            "Barlang: Száraz, szélvédett barlang, tökéletes egy jó alváshoz.",
        };

        // ---- Éjszakai események (N1) ----

        public static readonly string[] RainTexts =
        {
            "Éjjel leszakadt az ég. Reggelre minden edényed megtelt esővízzel.",
        };

        public static readonly string[] CoconutTexts =
        {
            "A szél éjjel lerázott pár kókuszdiót a fáról.",
        };

        public static readonly string[] CrateTexts =
        {
            "Holdfényben egy ládát látsz a hullámokban, nem messze a parttól.",
        };

        // ---- Nagy vihar (N4) ----

        public static readonly string[] StormWarnings =
        {
            "A madarak alacsonyan repülnek, a tenger nyugtalan. Két nap múlva nagy vihar jön!",
        };

        public static readonly string[] WreckTexts =
        {
            "A vihar egy hajóroncsot sodort a partra. Érdemes lenne átkutatni.",
        };

        // ---- Vége ----

        public static readonly string[] RescueTexts =
        {
            "Egy hajó észrevette a jelzőtüzet! Csónakot küldenek érted. Megmenekültél!",
        };

        public static readonly string[] GameOverTexts =
        {
            "Elfogyott az erőd. A sziget megtartott magának.",
            "Már nem volt erőd felkelni. A hullámok tovább mossák a partot.",
        };
    }
}
