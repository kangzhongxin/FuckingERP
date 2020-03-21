using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kzx.Common
{
    public class sysClass
    {
       
      
        public static DataSet dsParam = new DataSet();
         
        public static ProgressBar pBar; 
        public static DevExpress.XtraEditors.PanelControl pMsg;
        public static DevExpress.XtraEditors.LabelControl lblMsg;
        public static Timer timer;
        public static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };


        /// <summary>
        /// 根据Msg_ID取相应语言描述，获取为空时显示 emptyDisplayMsg
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="emptyDisplayMsg"></param>
        /// <returns></returns>
        public static string ssLoadMsgOrDefault(string msgID, string emptyDisplayMsg)
        {
            if (string.IsNullOrWhiteSpace(msgID))
                return emptyDisplayMsg;

            var msgText = string.Empty;
            if (SysVar.LanguageList.ContainsKey(msgID))
                msgText = SysVar.LanguageList[msgID];

            if (string.IsNullOrWhiteSpace(msgText))
                msgText = emptyDisplayMsg;

            return msgText;
        }
        /// <summary>  
        /// 获取汉字的首字母  
        /// </summary>  
        /// <param name="IndexTxt">汉字</param>  
        /// <returns>汉字的首字母</returns>  
        public static string GetChineseSpell(string IndexTxt)
        {
            string _Temp = null;
            for (int i = 0; i < IndexTxt.Length; i++)
                _Temp = _Temp + GetOneIndex(IndexTxt.Substring(i, 1));
            return _Temp;
        }

        /// <summary>  
        /// 单个汉字  
        /// </summary>  
        /// <param name="OneIndexTxt">汉字</param>  
        /// <returns>首拼</returns>  
        private static String GetOneIndex(String OneIndexTxt)
        {
            string sTmpStr = "";
            if (Convert.ToChar(OneIndexTxt) >= 0 && Convert.ToChar(OneIndexTxt) < 256)
                return OneIndexTxt;
            else
            {
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] unicodeBytes = Encoding.Unicode.GetBytes(OneIndexTxt);
                byte[] gb2312Bytes = Encoding.Convert(Encoding.Unicode, gb2312, unicodeBytes);
                try
                {
                    sTmpStr = GetX(Convert.ToInt32(
                   String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[0]) - 160)
                    + String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[1]) - 160)
                     ));
                    return sTmpStr;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        /// <summary>  
        /// 根据区位得到首字母  
        /// </summary>  
        /// <param name="GBCode">区位</param>  
        /// <returns></returns>  
        private static String GetX(int GBCode)
        {
            if (GBCode >= 1601 && GBCode < 1637) return "a";
            if (GBCode >= 1637 && GBCode < 1833) return "b";
            if (GBCode >= 1833 && GBCode < 2078) return "c";
            if (GBCode >= 2078 && GBCode < 2274) return "d";
            if (GBCode >= 2274 && GBCode < 2302) return "e";
            if (GBCode >= 2302 && GBCode < 2433) return "f";
            if (GBCode >= 2433 && GBCode < 2594) return "g";
            if (GBCode >= 2594 && GBCode < 2787) return "h";
            if (GBCode >= 2787 && GBCode < 3106) return "j";
            if (GBCode >= 3106 && GBCode < 3212) return "k";
            if (GBCode >= 3212 && GBCode < 3472) return "l";
            if (GBCode >= 3472 && GBCode < 3635) return "m";
            if (GBCode >= 3635 && GBCode < 3722) return "n";
            if (GBCode >= 3722 && GBCode < 3730) return "o";
            if (GBCode >= 3730 && GBCode < 3858) return "p";
            if (GBCode >= 3858 && GBCode < 4027) return "q";
            if (GBCode >= 4027 && GBCode < 4086) return "r";
            if (GBCode >= 4086 && GBCode < 4390) return "s";
            if (GBCode >= 4390 && GBCode < 4558) return "t";
            if (GBCode >= 4558 && GBCode < 4684) return "w";
            if (GBCode >= 4684 && GBCode < 4925) return "x";
            if (GBCode >= 4925 && GBCode < 5249) return "y";
            if (GBCode >= 5249 && GBCode <= 5589) return "z";
            if (GBCode >= 5601 && GBCode <= 8794)
            {
                String CodeData = "cjwgnspgcenegypbtwxzdxykygtpjnmjqmbsgzscyjsyyfpggbzgydywjkgaljswkbjqhyjwpdzlsgmr"
                + "ybywwccgznkydgttngjeyekzydcjnmcylqlypyqbqrpzslwbdgkjfyxjwcltbncxjjjjcxdtqsqzycdxxhgckbphffss"
                + "pybgmxjbbyglbhlssmzmpjhsojnghdzcdklgjhsgqzhxqgkezzwymcscjnyetxadzpmdssmzjjqjKzxcjjfwqjbdzbjgd"
                + "nzcbwhgxhqkmwfbpbqdtjjzkqhylcgxfptyjyKzxpsjlfchmqshgmmxsxjpkdcmbbqbefsjwhwwgckpylqbgldlcctnma"
                + "eddksjngkcsgxlhzaybdbtsdkdylhgymylcxpycjndqjwxqxfyyfjlejbzrwccqhqcsbzkymgplbmcrqcflnymyqmsqt"
                + "rbcjthztqfrxchxmcjcjlxqgjmshzkbswxemdlckfsydsglycjjssjnqbjctyhbftdcyjdgwyghqfrxwckqkxebpdjpx"
                + "jqsrmebwgjlbjslyysmdxlclqkxlhtjrjjmbjhxhwywcbhtrxxglhjhfbmgykldyxzpplggpmtcbbajjzyljtyanjgbj"
                + "flqgdzyqcaxbkclecjsznslKzxhlxlzcghbxzhznytdsbcjkdlzayffydlabbgqszkggldndnyskjshdlxxbcghxyggdj"
                + "mmzngmmccgwzszxsjbznmlzdthcqydbdllscddnlkjyhjsycjlkohqasdhnhcsgaehdaashtcplcpqybsdmpjlpcjaql"
                + "cdhjjasprchngjnlhlyyqyhwzpnccgwwmzffjqqqqxxaclbhkdjxdgmmydjxzllsygxgkjrywzwyclzmcsjzldbndcfc"
                + "xyhlschycjqppqagmnyxpfrkssbjlyxyjjglnscmhcwwmnzjjlhmhchsyppttxrycsxbyhcsmxjsxnbwgpxxtaybgajc"
                + "xlypdccwqocwkccsbnhcpdKzxnbcyytyckskybsqkkytqqxfcwchcwkelcqbsqyjqcclmthsywhmktlkjlychwheqjhtj"
                + "hppqpqscfymmcmgbmhglgsllysdllljpchmjhwljcyhzjxhdxjlhxrswlwzjcbxmhzqxsdzpmgfcsglsdymjshxpjxom"
                + "yqknmyblrthbcftpmgyxlchlhlzylxgsssscclsldclepbhshxyyfhbmgdfycnjqwlqhjjcywjztejjdhfblqxtqkwhd"
                + "chqxagtlxljxmsljhdzkzjecxjcjnmbbjcsfywkbjzghysdcpqyrsljpclpwxsdwejbjcbcnaytmgmbapclyqbclzxcb"
                + "nmsggfnzjjbzsfqyndxhpcqkzczwalsbccjxpozgwkybsgxfcfcdkhjbstlqfsgdslqwzkxtmhsbgzhjcrglyjbpmljs"
                + "xlcjqqhzmjczydjwbmjklddpmjegxyhylxhlqyqhkycwcjmyhxnatjhyccxzpcqlbzwwwtwbqcmlbmynjcccxbbsnzzl"
                + "jpljxKzxtzlgcldcklyrzzgqtgjhhgjljaxfgfjzslcfdqzlclgjdjcsnclljpjqdcclcjxmKzxftsxgcgsbrzxjqqcczh"
                + "gyjdjqqlzxjyldlbcyamcstylbdjbyregklzdzhldszchznwczcllwjqjjjkdgjcolbbzppglghtgzcygezmycnqcycy"
                + "hbhgxkamtxyxnbskKzxzgjzlqjdfcjxdygjqjjpmgwgjjjpkjsbgbmmcjssclpqpdxcdyykypcjddyygywchjrtgcnyql"
                + "dkljczzgzccjgdyksgpzmdlcphnjafKzxdjcnmwescsglbtzcgmsdllyxqsxsbljsbbsgghfjlwpmzjnlyywdqshzxtyy"
                + "whmcyhywdbxbtlmswyyfsbjcbdxxlhjhfpsxzqhfzmqcztqcxzxrdkdjhnnKzxqqfnqdmmgnydxmjgdhcdycbffallztd"
                + "ltfkmxqzdngeqdbdczjdxbzgsqqddjcmbkxffxmkdmcsychzcmljdjynhprsjmkmpcklgdbqtfzswtfgglyplljzhgjj"
                + "gypzltcsmcnbtjbhfkdhbKzxgkpbbymtdlsxsbnpdkleycjnycdykzddhqgsdzsctarlltkzlgecllkjljjaqnbdggghf"
                + "jtzqjsecshalqfmmgjnlyjbbtmlycxdcjpldlpcqdhsycbzsckbzmsljflhrbjsnbrgjhxpdgdjybzgdlgcsezgxlblg"
                + "yxtwmabchecmwyjKzxlljjshlgndjlslygkdzpzxjyKzxlpcxszfgwyydlyhcljscmbjhblyjlycblydpdqysxktbytdkd"
                + "xjypcnrjmfdjgklccjbctbjddbblblcdqrppxjcglzcshltoljnmdddlngkaqakgjgyhheznmshrphqqjchgmfprxcjg"
                + "dychghlyrzqlcngjnzsqdkqjymszswlcfqjqxgbggxmdjwlmcrnfkkfsyyljbmqammmycctbshcptxxzzsmphfshmclm"
                + "ldjfyqxsdyjdjjzzhqpdszglssjbckbxyqzjsgpsxjzqznqtbdkwxjkhhgflbcsmdldgdzdblzkycqnncsybzbfglzzx"
                + "swmsccmqnjqsbdqsjtxxmbldxcclzshzcxrqjgjylxzfjphymzqqydfqjjlcznzjcdgzygcdxmzysctlkphtxhtlbjxj"
                + "lxscdqccbbqjfqzfsltjbtkqbsxjjljchczdbzjdczjccprnlqcgpfczlclcxzdmxmphgsgzgszzqjxlwtjpfsyaslcj"
                + "btckwcwmytcsjjljcqlwzmalbxyfbpnlschtgjwejjxxglljstgshjqlzfkcgnndszfdeqfhbsaqdgylbxmmygszldyd"
                + "jmjjrgbjgkgdhgkblgkbdmbylxwcxyttybkmrjjzxqjbhlmhmjjzmqasldcyxyqdlqcafywyxqhz";
                String _gbcode = GBCode.ToString();
                int pos = (Convert.ToInt16(_gbcode.Substring(0, 2)) - 56) * 94 + Convert.ToInt16(_gbcode.Substring(_gbcode.Length - 2, 2));
                return CodeData.Substring(pos - 1, 1);
            }
            return " ";
        }

     
         
    }
}
