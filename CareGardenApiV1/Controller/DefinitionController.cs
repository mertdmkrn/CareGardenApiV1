using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using CareGardenApiV1.Repository.Abstract;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class DefinitionController : ControllerBase
    {
        private readonly CareGardenApiDbContext _context;
        private readonly IBusinessPropertiesService _businessPropertiesService;
        private readonly IBusinessService _businessService;


        public DefinitionController(CareGardenApiDbContext context, IBusinessService businessService, IBusinessPropertiesService businessPropertiesService)
        {
            _context = context;
            _businessService = businessService;
            _businessPropertiesService = businessPropertiesService;
        }

        /// <summary>
        /// Get Cities
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("definition/getcities")]
        public async Task<IActionResult> GetCities()
        {
            ResponseModel<List<string>> response = new ResponseModel<List<string>>();
            response.Data = Constants.Cities;

            return Ok(response);
        }

        /// <summary>
        /// Get Privacy Policy
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("definition/privacypolicy")]
        public async Task<IActionResult> PrivacyPolicy()
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Data = "<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>REGARDING THE PROTECTION AND PROCESSING OF PERSONAL DATA clarification text&nbsp; AND PRIVACY POLICY</strong></span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">As BeeOs Yazılım Anonim Şirketi, we attach importance to the privacy and security of your personal data that you share with us; We show maximum sensitivity to the principle of protecting the fundamental rights and freedoms guaranteed by the Constitution, especially the privacy of private life. Accordingly, in accordance with the disclosure obligation under the Law on the Protection of Personal Data No. 6698, which entered into force with the Official Gazette dated 07.04.2016, we submit the following matters for your information.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>1. What are the Definitions of Technical Terms in the Illumination Text?</strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Explicit Consent:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> It means consent on a specific subject based on the information and expressed with free will.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Anonymization:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> It means making personal data incapable of being associated with an identified or identifiable natural person in any way, even by matching with other data.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Relevant Person:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> It means the natural person whose personal data is processed.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Law:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> Law on Protection of Personal Data No. 6698, which entered into force with the Official Gazette dated 07.04.2016</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Personal Data:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> Means any information relating to an identified or identifiable natural person.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Processing of Personal Data:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> Obtaining, recording, storing, preserving, changing, rearranging, disclosing, transferring, taking over, and making available personal data by fully or partially automatic or non-automatic means provided that it is a part of any data recording system, means all kinds of operations performed on data such as classification or prevention of use.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Board: </strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">It means the Personal Data Protection Board.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Institution:</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"> means the Personal Data Protection Authority.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Data Processor</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">: It means the natural or legal person who processes personal data on behalf of the data controller based on the authority given by him.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Data Registration System</strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">: It means the registration system in which personal data is processed and structured according to specific criteria.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>Data Controller: </strong></span></span></span><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">It means the natural or legal person who determines the purposes and means of processing personal data and is responsible for establishing and managing the data recording system.</span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>2. Who is the Data Controller?</strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">Data Controller regarding all kinds of personal data that you share through www.caregarden.app site and CareGarden application during the process of becoming a member of the site, to the Istanbul Trade Registry whose head office address is Merdivenk&ouml;y Mahallesi Dikyol Sokak No 2 B Blok Floor 14 Flat 142 G&ouml;ztepe, Kadık&ouml;y, Istanbul 107998-5 It is an BeeOs Yazılım Anonim Şirketi registered with no.su.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong>3. For What Purposes Do We Process Your Personal Data?</strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">By our company, from parties such as website and/or application users, customers, employees, potential customers, potential employees, potential users, business partners and suppliers, identity information, contact information, customer information, customer transaction information, transaction security information, legal action. Personal data can be collected in categories such as compliance information and marketing sales information.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">Personal data processed through www.caregarden.app and CareGarden application are data related to transaction security in case of realization of name, surname, e-mail address, password and online payment transactions. If the Related Person prefers, it is also possible to sign up from the person&#39;s social media accounts. In this case, the user name of the relevant person&#39;s social account is also processed. We would like to emphasize that we do not have access to the password and other confidential information of the relevant social media account.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">Collected Personal Data is processed for the purposes listed below.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Carrying out membership procedures to the site and/or application and ensuring that members have access to all kinds of information, documents, participation, transportation, accommodation and other features about the relevant organization and shared by the organization,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; To offer you special advertisements, campaigns, advantages and other benefits for sales, promotion and marketing activities to increase the quality of services and products, information processing requirements, the necessity of support services, conveying the necessary information about these services and products, measuring customer satisfaction , complaint management, receiving your opinions and suggestions about new services and products, and contacting and informing members when necessary,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Increasing the service quality and making the best use of the services provided through the website and the application by the Relevant Person, detecting system errors and monitoring performance, improving system operation, providing maintenance and support services and backup services,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; If the online payment option is used, payment transactions are carried out in accordance with the relevant legislation,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Conducting company activities in accordance with the law and relevant legislation,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Execution of the financial and accounting works of the company,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Follow-up and execution of the legal affairs of the company,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Execution and supervision of the company&#39;s business activities,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Execution of the company&#39;s storage and archive activities,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Execution of the company&#39;s strategic planning and R&amp;D activities,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Execution of the company&#39;s investment processes,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">&bull; Execution of the company&#39;s management activities</span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong><u>4. What Legal Grounds Do We Rely on While Processing Your Personal Data?</u></strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">While processing your personal data, we base on the provisions of the Law on Protection of Personal Data No. 6698, related regulations and communiqu&eacute;s, Board decisions, Regulation on the Procedures and Principles Regarding the Regulation of Publications Made on the Internet No. 26716 and other relevant legislation and case law, in particular the provisions of the Constitution regarding fundamental rights and freedoms.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">However, the Company abides by the following principles adopted in the Law with great care and diligence:</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Personal data processing activities comply with the Law and honesty,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Ensuring that personal data is accurate and up-to-date when necessary,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Processing personal data for specific, explicit and legitimate purposes,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Being connected, limited and restrained with the purpose for which they are processed,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Storage for the period required by the relevant legislation or for the purpose for which they are processed.</span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong><u>5. How Long Do We Retain Your Personal Data?</u></strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">BeeOs Yazılım Anonim Şirketi keeps the Personal Data subject to this Clarification Text for the period stipulated in the relevant legislation or for the period required for the purpose for which they are processed. In this context, it first determines whether a period is foreseen for the storage of personal data in the relevant legislation and acts in accordance with this period if a period is determined. If a period is not specified, it stores the personal data for the period required for the purpose they are processed.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">Personal data is deleted, destroyed or anonymized if the period expires or the reasons for its processing disappear.</span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong><u>6. In Which Ways Do We Collect Your Personal Data?</u></strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">Your Personal Data may be collected verbally, in writing or electronically, through automatic or non-automatic methods, through our website, social media channels, parties with whom we have business relations and/or services that are complementary to our activities, contracted organizations and other similar channels. Your personal data is stored in electronic and/or physical media. To prevent your personal data provided and stored by our Company from being exposed to unauthorized access, manipulation, loss or damage, all technical measures foreseen and required in the relevant legislation are taken, and security improvements are implemented.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\"><strong><u>7. To Whom Can We Transfer Your Personal Data?</u></strong></span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">Your Personal Data, within the framework of the purposes set out in Article 3,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Company shareholders, employees, business partners,</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- Person or third parties from whom the Company receives services regarding its commercial activities, and legal, financial and tax consultants, auditors, consultants, organizations or individuals</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">- It can be transferred to regulatory and supervisory institutions and public institutions or organizations expressly authorized to request personal data in-laws.</span></span></span></p>\n\n<p style=\"margin-left:24px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\"><span style=\"color:#000000\">In case your Personal Data is transferred abroad, within the scope of Article 9 of the Law, it is necessary to have adequate protection in the foreign country to which the personal data will be transferred; in case of lack of sufficient protection, obtaining explicit consent from the data subject, data controllers in Turkey and the relevant foreign country undertake in writing to provide adequate protection. And the permission of the Personal Data Protection Authority will be respected.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\"><strong><u>8. Under Which Conditions Can Your Personal Data Be Processed Without Explicit Consent Requirement?</u></strong></span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">Your Personal Data may be processed without your explicit consent in the following cases:</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Explicitly stipulated in-laws.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- It is compulsory to protect the life or physical integrity of the person or another person who cannot express his consent due to actual impossibility or whose consent is not legally recognized.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- It is necessary to process the parties&#39; personal data to the contract, provided that it is directly related to the performance of the contract.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- It is mandatory to fulfil our legal obligations as a data controller;</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- The person concerned has been made public by himself.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Data processing is mandatory for establishing, exercising or protecting a right.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Data processing is mandatory for the data controller&#39;s legitimate interests, provided that it does not harm the fundamental rights and freedoms of the data subject.</span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\"><strong><u>9. What are the Rights of Personal Data Owners Enumerated in Article 11 of the Law?</u></strong></span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">Personal Data owners have the following rights in accordance with Article 11 of the Law.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Learning whether personal data is processed or not,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- If personal data has been processed, requesting information about it,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- To learn the purpose of processing personal data and whether they are used in accordance with the purpose,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- To know the third parties to whom personal data is transferred in the country or abroad,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Requesting correction of personal data in case of incomplete or incorrect processing,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Requesting correction of personal data in case of incomplete or incorrect processing and requesting notification of the transaction made within this scope to the third parties to whom the personal data has been transferred,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Requesting the deletion or destruction of personal data if the reasons requiring its processing cease to exist, even though it has been processed in accordance with the provisions of the law and other relevant laws, and requesting that the transaction carried out within this scope be notified to the third parties to whom the personal data has been transferred,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- Objecting to the emergence of a result against the person himself by analyzing the processed data exclusively through automated systems,</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- To request compensation for the damage in case of loss due to unlawful processing of personal data.</span></span></span></p>\n\n<p>&nbsp;</p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\"><strong><u>10. In What Way Can Personal Data Owners Use Their Rights?</u></strong></span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">In case the Relevant Person requests information on the issues mentioned above;</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- The Data Owner can fill in the Application Form on our website and/or the application and send it to us.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- You can forward your questions about the subject to the e-mail address &quot;support@caregarden.app&quot;.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- He can convey the matter he wants to receive information on by sending a notification through the notary public.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">- May prefer other methods determined and/or determined by the Board.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\">The application request of the Related Person will be responded to as soon as possible and within thirty (30) days at the latest in accordance with the law. However, suppose the transaction requires an additional cost. In that case, our Company reserves the right to charge a fee based on the tariff determined by the Personal Data Protection Board in accordance with the Law.</span></span></span></p>\n\n<p style=\"margin-right:-9px; text-align:justify\"><span style=\"font-size:11pt\"><span style=\"font-family:'Times New Roman'\"><span style=\"color:#000000\"><strong>* In this Clarification Text and Privacy Policy, changes can be made without any notification to the users due to legislative changes, current case law provisions, innovations in judicial decisions and other reasons. For this reason, we recommend that the text be reviewed and checked periodically.&nbsp;</strong></span></span></span></p>\n\n<p>&nbsp;</p>\n";

            return Ok(response);
        }

        ///// <summary>
        ///// Get Privacy Policy
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("definition/setbusinesses")]
        //public async Task<IActionResult> SetBusinesses()
        //{
        //    List<string> cityList = new List<string>() { "istanbul", "ankara", "izmir", "bursa", "adana", "antalya", "kocaeli", "sakarya", "konya", "kastamonu" };
        //    List<string> categoryList = new List<string>() { "sac-kesimi", "sac-boyama", "manikur", "spalar", "saglikli-yasam", "dovme-tattoo-merkezleri" };
        //    string baseUrl = "https://www.kolayrandevu.com/{0}/{1}/{2}";
        //    List<string> additionalLinks = new List<string>();

        //    Dictionary<string, List<string>> urlDict = new Dictionary<string, List<string>>();

        //    var businesses = await _businessService.GetBusinessListForCache();

        //    foreach (string city in cityList)
        //    {
        //        foreach (string category in categoryList)
        //        {
        //            for (int i = 0; i < 5; i++)
        //            {
        //                var url = string.Format(baseUrl, category, city, (i + 1).ToString());
        //                var htmlDoc = new HtmlWeb().Load(url);

        //                var links = htmlDoc.DocumentNode.SelectNodes("//h3[@class='listing-salon-title']/a");

        //                if (links != null)
        //                {
        //                    foreach (var link in links)
        //                    {
        //                        string href = link.Attributes["href"].Value;

        //                        if (!additionalLinks.Contains(href))
        //                        {
        //                            if (urlDict.Count() == 0 || !urlDict.ContainsKey(url))
        //                            {
        //                                urlDict.Add(url, new List<string>() { href });
        //                            }
        //                            else
        //                            {
        //                                urlDict[url].Add(href);
        //                            }

        //                            additionalLinks.Add(href);
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }

        //    List<Business> businessList = new List<Business>();

        //    foreach (var kvp in urlDict)
        //    {
        //        foreach (var link in kvp.Value)
        //        {
        //            Business business = new Business();

        //            var htmlDoc = new HtmlWeb().Load(link);

        //            business.name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']")?.InnerText.Replace(" &amp; ", " ");

        //            if (business.name.IsNullOrEmpty() || businesses.Any(x => x.name.Equals(business.name))) continue;

        //            business.name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']")?.InnerText.Replace("&amp;", " ").Replace("  ", " ");
        //            business.telephone = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='mobil-callcenter-number']")?.Attributes["href"]?.Value.Replace("tel://", "") ?? "+905467335939";
        //            business.description = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"single_tour_desc\"]/div/div[@class=\"col-md-9\"]/p")?.InnerText.Replace("&amp;", " ").Replace("  ", " ");
        //            business.descriptionEn = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"single_tour_desc\"]/div/div[@class=\"col-md-9\"]/p")?.InnerText;
        //            business.city = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='addressRegion']")?.InnerText;
        //            business.province = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='addressLocality']")?.InnerText;
        //            business.latitude = Convert.ToDouble(htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='latitude']")?.Attributes["content"]?.Value.Replace(".", ","));
        //            business.longitude = Convert.ToDouble(htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='longitude']")?.Attributes["content"]?.Value.Replace(".", ","));
        //            business.logoUrl = htmlDoc.DocumentNode.SelectSingleNode("//img[@itemprop='logo']")?.Attributes["data-src-defer"]?.Value;
        //            business.address = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='streetAddress']")?.InnerText + " " + business.city + " " + business.province; 
        //            business.isActive = true;
        //            business.verified = true;
        //            business.createDate = DateTime.Now;
        //            business.updateDate = business.createDate;
        //            business.workingGenderType = WorkingGenderType.Everyone;
        //            business.email = business.name.Split(" ").FirstOrDefault().ToLower() + "123@gmail.com";
        //            business.password = "12345678";
        //            business.officialHolidayAvailable = true;

        //            await _context.Businesses.AddAsync(business);
        //            await _context.SaveChangesAsync();

        //            var hours = htmlDoc.DocumentNode.SelectNodes("//meta[@itemprop='openingHours']");

        //            if (hours.Count() > 0)
        //            {
        //                BusinessWorkingInfo businessWorkingInfo = new BusinessWorkingInfo();

        //                businessWorkingInfo.mondayWorkHours = hours[0].Attributes["content"]?.Value?.Substring(3);
        //                businessWorkingInfo.tuesdayWorkHours = hours.Count > 1 ? hours[1].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.wednesdayWorkHours = hours.Count > 2 ? hours[2].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.thursdayWorkHours = hours.Count > 3 ? hours[3].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.fridayWorkHours = hours.Count > 4 ? hours[4].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.saturdayWorkHours = hours.Count > 5 ? hours[5].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.sundayWorkHours = hours.Count > 6 ? hours[6].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.businessId = business.id;

        //                await _context.BusinessWorkingInfos.AddAsync(businessWorkingInfo);
        //                await _context.SaveChangesAsync();
        //            }

        //            var serviceNameNodes = htmlDoc.DocumentNode.SelectNodes("//tr[@class='favhizmet-liste-select click_service']");
        //            List<BusinessServiceModel> businessServiceModels = new List<BusinessServiceModel>();

        //            if (serviceNameNodes != null && serviceNameNodes.Count() > 0)
        //            {

        //                for (int i = 0; i < serviceNameNodes.Count; i++)
        //                {
        //                    var node = serviceNameNodes[i];
        //                    HtmlDocument doc = new HtmlDocument();
        //                    doc.LoadHtml(node.OuterHtml);

        //                    BusinessServiceModel businessServiceModel = new BusinessServiceModel();
        //                    businessServiceModel.name = doc.DocumentNode.SelectSingleNode("//tr[@class='favhizmet-liste-select click_service']//label[@class='hizmet-listesi-label']")?.InnerText;

        //                    if (businessServiceModel.name != null && !businessServiceModels.Exists(x => x.name == businessServiceModel.name))
        //                    {
        //                        businessServiceModel.serviceId = getServiceId(link, businessServiceModel.name);
        //                        businessServiceModel.nameEn = doc.DocumentNode.SelectSingleNode("//tr[@class='favhizmet-liste-select click_service']//label[@class='hizmet-listesi-label']")?.InnerText;
        //                        businessServiceModel.price = Convert.ToDouble(doc.DocumentNode.SelectSingleNode("//tr[@class='favhizmet-liste-select click_service']//span")?.InnerText.Split("~").LastOrDefault().Replace("TL", "").Trim());

        //                        if (businessServiceModel.price == 0)
        //                            businessServiceModel.price = 150;

        //                        businessServiceModel.isPopular = i % 3 == 0 ? true : false;
        //                        businessServiceModel.minDuration = i % 2 == 0 ? 30 : 45;
        //                        businessServiceModel.maxDuration = i % 5 == 0 ? 0 : i % 2 == 0 ? 45 : 60;
        //                        businessServiceModel.businessId = business.id;
        //                        businessServiceModel.spot = "Spot";
        //                        businessServiceModel.spotEn = "Spot EN";

        //                        businessServiceModels.Add(businessServiceModel);
        //                    }
        //                }

        //                await _context.BusinessServices.AddRangeAsync(businessServiceModels);
        //                await _context.SaveChangesAsync();
        //            }

        //            List<BusinessGallery> businessGalleries = new List<BusinessGallery>();
        //            var profilePhotoNode = htmlDoc.DocumentNode.SelectSingleNode("//img[@id='main-image']");

        //            if (profilePhotoNode != null)
        //            {
        //                BusinessGallery photo = new BusinessGallery();

        //                photo.imageUrl = profilePhotoNode.Attributes["src"]?.Value;
        //                photo.isProfilePhoto = true;
        //                photo.sortOrder = 1;
        //                photo.businessId = business.id;

        //                businessGalleries.Add(photo);
        //            }

        //            var galleryNodes = htmlDoc.DocumentNode.SelectNodes("//a[@data-lightbox='galeri']");

        //            if (galleryNodes != null && galleryNodes.Count() > 0)
        //            {
        //                for (int i = 1; i < galleryNodes.Count; i++)
        //                {
        //                    var node = galleryNodes[i];
        //                    BusinessGallery photo = new BusinessGallery();

        //                    photo.imageUrl = node.Attributes["href"]?.Value;
        //                    photo.isSliderPhoto = i < 4 ? true : false;
        //                    photo.sortOrder = i + 1;
        //                    photo.businessId = business.id;

        //                    businessGalleries.Add(photo);
        //                }
        //            }

        //            await _context.BusinessGalleries.AddRangeAsync(businessGalleries);
        //            await _context.SaveChangesAsync();

        //            var workerNodes = htmlDoc.DocumentNode.SelectNodes("//div[@itemprop='employee']");
        //            var serviceIdList = businessServiceModels.Select(x => x.id).ToList();

        //            if (workerNodes != null && workerNodes.Count() > 0)
        //            {
        //                List<Worker> workers = new List<Worker>();

        //                for (int i = 0; i < workerNodes.Count; i++)
        //                {
        //                    var node = workerNodes[i];
        //                    HtmlDocument doc = new HtmlDocument();
        //                    doc.LoadHtml(node.OuterHtml);

        //                    Worker worker = new Worker();
        //                    worker.name = doc.DocumentNode.SelectSingleNode("//img[@class='personel-image']")?.Attributes["alt"]?.Value;

        //                    if (worker.name != null && !workers.Exists(x => x.name == worker.name))
        //                    {
        //                        worker.title = "Takım Üyesi";
        //                        worker.path = doc.DocumentNode.SelectSingleNode("//img[@class='personel-image']")?.Attributes["data-src-defer"]?.Value;
        //                        worker.isActive = true;
        //                        worker.isAvailable = true;
        //                        worker.businessId = business.id;
        //                        worker.serviceIds = string.Join(";", serviceIdList.Skip(i % 2 == 0 ? serviceIdList.Count() / 2 : 0).Take(serviceIdList.Count() / 2));

        //                        workers.Add(worker);
        //                    }
        //                }

        //                await _context.Workers.AddRangeAsync(workers);
        //                await _context.SaveChangesAsync();
        //            }

        //            var commentNodes = htmlDoc.DocumentNode.SelectNodes("//div[@itemprop='review']");

        //            if (commentNodes != null && commentNodes.Count > 0)
        //            {
        //                List<Comment> comments = new List<Comment>();

        //                List<Guid> userIds = new List<Guid>()
        //                {
        //                    new Guid("304d4e72-e439-43b0-9025-aab46041aabd"),
        //                    new Guid("815c9c66-6117-4e11-92e0-40d1f640261e"),
        //                    new Guid("0b3e3b42-971d-4caa-8bad-309d540212fc"),
        //                    new Guid("987c12fd-b14b-47e2-815b-3cf335f02d78"),
        //                    new Guid("9d807e31-483b-48ce-aea5-a2106d9df526"),
        //                    new Guid("f3530132-c501-4f51-8acf-e0e716f5f95b"),
        //                    new Guid("72e5ce1e-7ad5-423b-af1b-7d9ef4a25a54"),
        //                };

        //                for (int i = 0; i < commentNodes.Count; i++)
        //                {
        //                    var node = commentNodes[i];
        //                    HtmlDocument doc = new HtmlDocument();
        //                    doc.LoadHtml(node.OuterHtml);

        //                    Comment comment = new Comment();
        //                    comment.comment = doc.DocumentNode.SelectSingleNode("//p[@itemprop='reviewBody']")?.InnerHtml;

        //                    if (comment.comment != null && !comments.Exists(x => x.comment == comment.comment))
        //                    {
        //                        if (comment.comment.Length > 300)
        //                            comment.comment = comment.comment.Substring(0, 299);

        //                        comment.point = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//span[@itemprop='ratingValue']")?.InnerHtml);
        //                        var dateStr = doc.DocumentNode.SelectSingleNode("//meta[@itemprop='datePublished']")?.Attributes["content"]?.Value;

        //                        comment.createDate = Convert.ToDateTime(dateStr);
        //                        comment.updateDate = comment.createDate;
        //                        comment.userId = userIds[i % 6];
        //                        comment.businessId = business.id;
        //                        comment.commentType = CommentType.User;

        //                        comments.Add(comment);
        //                    }
        //                }

        //                await _context.Comments.AddRangeAsync(comments);
        //                await _context.SaveChangesAsync();
        //            }

        //            await _businessPropertiesService.SaveStaticBusinessPropertiesAsync(business.id);

        //            Console.WriteLine(business.name + " eklendi.");
        //        }
        //    }

        //    await _businessService.GetBusinessListForCache(false);

        //    return Ok();
        //}


        //public static Guid getServiceId(string link, string name)
        //{
        //    if (name.ToLower().Contains("boyama"))
        //    {
        //        return new Guid("d10a8836-7011-4017-b456-7aab046ae915");
        //    }

        //    if (name.ToLower().Contains("kesim"))
        //    {
        //        return new Guid("f5b4a17b-2fb8-4b66-8aff-16ed4e77053d");
        //    }

        //    if (name.ToLower().Contains("sakal"))
        //    {
        //        return new Guid("fa88ef64-0fbd-45a5-97e2-26c4f2da8339");
        //    }

        //    if (name.ToLower().Contains("kaş") || name.ToLower().Contains("kirpik"))
        //    {
        //        return new Guid("059b3bce-fdec-48e7-8200-3a101988c014");
        //    }

        //    if (name.ToLower().Contains("mani"))
        //    {
        //        return new Guid("27bcb7ae-9038-440e-8bf8-edeafdadc20f");
        //    }

        //    if (name.ToLower().Contains("pedi"))
        //    {
        //        return new Guid("520e8c0e-5911-43e1-bfbb-f9b2f0023ede");
        //    }

        //    if (name.ToLower().Contains("oje"))
        //    {
        //        return new Guid("3016aa94-a5cf-46a4-804a-cb71c37473eb");
        //    }

        //    if (name.ToLower().Contains("cilt") || name.ToLower().Contains("dudak"))
        //    {
        //        return new Guid("dc54d9c6-cfba-4c45-8fef-872325c00581");
        //    }

        //    if (name.ToLower().Contains("makyaj"))
        //    {
        //        return new Guid("e911185f-d392-4d2c-83f2-d8de1efdbf35");
        //    }

        //    if (name.ToLower().Contains("diş"))
        //    {
        //        return new Guid("a1fb5b25-f34b-43ee-be9c-b1a808a7491c");
        //    }

        //    if (name.ToLower().Contains("spa") || link.Contains("spalar"))
        //    {
        //        return new Guid("7422f809-e063-41a0-a411-a2ae956c3df1");
        //    }

        //    if (name.ToLower().Contains("epilas") || name.ToLower().Contains("ada"))
        //    {
        //        return new Guid("24f962c1-64ef-43f9-bbf5-323d2eda99a1");
        //    }

        //    return new Guid("f5b4a17b-2fb8-4b66-8aff-16ed4e77053d");
        //}
    }
}
