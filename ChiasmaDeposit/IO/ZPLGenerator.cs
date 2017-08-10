using System;
using ChiasmaDeposit.Properties;

namespace Molmed.ChiasmaDep.IO
{
    public class ZPLGenerator
    {

        public ZPLGenerator()
        {
        }

        public bool GenerateAlphaNumeric(string targetFileName, string text, string fontName, int magnificationFactor, int offsetX, int offsetY, int copies)
        {
            int i;
            string script;
            ChiasmaDep.IO.FileServer fs;

            try
            {
                script = "";

                for (i = 1; i <= copies; i++)
                {
                    script = script + this.GenerateAlphaNumericSingle(text, fontName, magnificationFactor, offsetX, offsetY);
                }

                fs = new ChiasmaDep.IO.FileServer();
                return fs.SaveString(script, targetFileName);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }


        private string GenerateAlphaNumericSingle(string text, string fontName, int magnificationFactor, int offsetX, int offsetY)
        {
            string ret;
            string[] lines;
            char[] delim;
            int i, height, width;
            int lineOffsetY;

            try
            {
                delim = new Char[1];
                delim[0] = Convert.ToChar(10);
                lines = text.Split(delim);

                height = this.GetFontBaseHeight(fontName) * magnificationFactor;
                width = this.GetFontBaseWidth(fontName) * magnificationFactor;

                ret = "";
                ret = ret + "^XA";
                for (i = 0; i <= lines.GetUpperBound(0); i++)
                {
                    lineOffsetY = Convert.ToInt32(offsetY + i * height * 0.7);
                    ret = ret + "^A" + fontName + height + "," + width;
                    ret = ret + "^FO" + offsetX.ToString() + "," + Convert.ToString(lineOffsetY);
                    ret = ret + "^FD" + lines[i] + "^FS";
                }
                ret = ret + "^XZ";

                return ret;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GenerateBarCode128cSingle(string barCode, int height, int width, int offsetX, int offsetY, string text, LabelPrinter.BarCodeTextPositions textPos)
        {
            string ret;
            int textHeight, textWidth;

            string fontName;
            int magFactor, spacing;
            int textFieldBlockWidthPoints, textFieldMaxLines;
            int textFieldLineSpacePoints, textFieldCharactersPerLine;

            try
            {
                fontName = Settings.Default.PlateLabelFontName;
                magFactor = Settings.Default.PlateLabelFontMagnFactor;
                spacing = Settings.Default.PlateLabelBarCodeYSpacingPoints;
                textFieldBlockWidthPoints = Settings.Default.PlateLabelTextBlockWidthPoints;
                textFieldMaxLines = Settings.Default.PlateLabelTextMaxLines;
                textFieldLineSpacePoints = Settings.Default.PlateLabelTextLineSpacePoints;
                textFieldCharactersPerLine = Settings.Default.PlateLabelTextCharsPerLine;

                textHeight = magFactor * this.GetFontBaseHeight(fontName);
                textWidth = magFactor * this.GetFontBaseWidth(fontName);

                ret = "";
                ret = ret + "^XA";
                ret = ret + "^LH" + offsetX.ToString() + "," + offsetY.ToString();

                //If the text should be above the bar code, print the text first.
                if (textPos == LabelPrinter.BarCodeTextPositions.Above)
                {
                    ret = ret + "^A" + fontName + textHeight.ToString() + "," + textWidth.ToString();
                    ret = ret + "^FO" + "0,0";
                    ret = ret + "^FD" + text + "^FS";
                }

                //Print the actual bar code.
                ret = ret + "^FO" + "0," + Convert.ToString(textHeight + spacing);
                ret = ret + "^BY" + width.ToString();
                ret = ret + "^BCN," + Convert.ToString(height) + ",N,N,N";
                ret = ret + "^FD>;" + barCode + "^FS";

                //If the text should be on the right-hand side of the bar code, print the text now.
                if (textPos == LabelPrinter.BarCodeTextPositions.Right)
                {
                    if (width <= 4)
                    {
                        ret = ret + "^FO" + Convert.ToString(barCode.Length * width * 7 + 120) + "," + Convert.ToString(textHeight + spacing);
                    }
                    else
                    {
                        ret = ret + "^FO" + Convert.ToString(barCode.Length * width * 7 + 200) + "," + Convert.ToString(textHeight + spacing);
                    }
                    ret = ret + "^A" + fontName + textHeight.ToString() + "," + textWidth.ToString();
                    ret = ret + "^FB" + textFieldBlockWidthPoints.ToString() + "," + textFieldMaxLines.ToString() + "," + textFieldLineSpacePoints.ToString() + ",";
                    ret = ret + "^FD" + FormatText(text, "\\&", textFieldCharactersPerLine) + "^FS";
                }

                ret = ret + "^XZ";

                return ret;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public Boolean GenerateCode128c(String targetFileName, LabelPrinter.BarCodeLabel[] barCodeLabelArray, int height, int width, int offsetX, int offsetY, LabelPrinter.BarCodeTextPositions textPos)
        {
            int i;
            string script;
            ChiasmaDep.IO.FileServer fs;

            script = "";

            for (i = 0; i <= barCodeLabelArray.GetUpperBound(0); i++)
            {
                script = script + this.GenerateBarCode128cSingle(barCodeLabelArray[i].BarCode, height, width, offsetX, offsetY, barCodeLabelArray[i].Text, textPos);
            }

            fs = new ChiasmaDep.IO.FileServer();
            return fs.SaveString(script, targetFileName);
        }

        private int GetFontBaseHeight(string fontName)
        {
            switch (fontName.ToUpper())
            {
                case "A":
                    return 9;
                case "B":
                    return 11;
                case "C":
                    return 18;
                case "D":
                    return 18;
                case "E":
                    return 28;
                case "F":
                    return 26;
                case "G":
                    return 60;
                case "H":
                    return 21;
                default:
                    throw (new Exception("Invalid font name. Cannot determine font base height."));
            }
        }

        private int GetFontBaseWidth(string fontName)
        {
            switch (fontName.ToUpper())
            {
                case "A":
                    return 5;
                case "B":
                    return 7;
                case "C":
                    return 10;
                case "D":
                    return 10;
                case "E":
                    return 15;
                case "F":
                    return 13;
                case "G":
                    return 40;
                case "H":
                    return 13;
                default:
                    throw (new Exception("Invalid font name. Cannot determine font base width."));
            }
        }

        private string FormatText(string text, string newLineCode, int charactersPerLine)
        {
            char[] charArray;
            int counter = 0;
            string formattedText = "";

            charArray = text.ToCharArray();
            foreach (char c in charArray)
            {
                if (++counter >= charactersPerLine)
                {
                    formattedText += (c.ToString() + newLineCode);
                    counter = 0;
                }
                else
                {
                    formattedText += c.ToString();
                }
            }

            return formattedText;
        }

        public bool ValidateBarCode128c(String barCode)
        {
            //Returns true if the bar code is printable in the Code 128c format,
            //otherwise false.
            int i;

            //Check for even figure pairs.
            if (barCode.Length % 2 != 0)
            {
                return false;
            }

            //Check for numbers.
            for (i = 0; i < barCode.Length; i++)
            {
                if ((barCode[i] < 48) || (barCode[i] > 57))
                {
                    return false;
                }
            }

            return true;
        }

    }
}