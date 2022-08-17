/////////////////////////////////////////////////////////////////////////////// 
// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
// All rights reserved. 
// 
// This software and its documentation and related materials are owned by 
// the Alliance. The software may only be incorporated into application 
// programs owned by members of the Alliance, subject to a signed 
// Membership Agreement and Supplemental Software License Agreement with the
// Alliance. The structure and organization of this software are the valuable  
// trade secrets of the Alliance and its suppliers. The software is also 
// protected by copyright law and international treaty provisions. Application  
// programs incorporating this software must include the following statement 
// with their copyright notices:
//   
//   This application incorporates Teigha(R) software pursuant to a license 
//   agreement with Open Design Alliance.
//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
//   All rights reserved.
//
// By use of this software, its documentation or related materials, you 
// acknowledge and accept the above terms.
///////////////////////////////////////////////////////////////////////////////

using System;
using Teigha.Core;
using Teigha.TG;
using System.Collections.Generic;
using System.Diagnostics;

class OdaDgnSampleDrawableOverrule : OdGiDrawableOverrule
{
	public override bool worldDraw(OdGiDrawable pSubject, OdGiWorldDraw wd)
	{
		// skip rendering any element having this overruling
		OdGiGeometry geom = wd.geometry();
		OdGiSubEntityTraits traits = wd.subEntityTraits();
		wd.subEntityTraits().setTrueColor(new OdCmEntityColor(192, 32, 255));
		//wd.subEntityTraits().setFillType(OdGiFillType.kOdGiFillAlways);
		//wd.subEntityTraits().setColor((ushort)OdCmEntityColor.ACIcolorMethod.kACIBlue);
		OdDgArc3d arc = (OdDgArc3d)pSubject;
		if (arc != null)
		{
			OdGePoint3d pt1 = arc.getOrigin();
			//OdGePoint3d pt2 = pt1;
			//pt2.set(pt1.x + arc.getPrimaryAxis(), pt1.y, pt1.z);
			geom.circle(pt1, arc.getPrimaryAxis(), arc.getNormal());
		}
		return true;
	}
};

abstract class OdExDgnDumper
{
	public abstract void dumpFieldName(string fieldName);
	public abstract void dumpFieldValue(string fieldName);

	// output for different types
	public void writeFieldValue(string name, string value)
	{
		dumpFieldName(name);
		dumpFieldValue(value);
	}
	public void writeFieldValue(string name, UInt16 value)
	{
		dumpFieldName(name);
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValueHex(string name, UInt16 value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:X}", value));
	}
	public void writeFieldValue(string name, UInt32 value)
	{
		dumpFieldName(name);
		if (value == UInt32.MaxValue)
		{
			dumpFieldValue("-1");
		}
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValueHex(string name, UInt32 value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:X6}", value));
	}
	public void writeFieldValueHex(string name, Int32 value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:X}", value));
	}
	public void writeFieldValue(string name, double value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:G5}", value));
	}
	public void writeFieldValue(string name, OdGePoint2d value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:G4}, {1:G4}", value.x, value.y));
	}
	public void writeFieldValue(string name, OdGePoint3d value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:G4}, {1:G4}, {2:G4}", value.x, value.y, value.z));
	}
	public void writeFieldValue(string name, OdGeVector3d value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:G4}, {1:G4}, {2:G4}", value.x, value.y, value.z));
	}
	public void writeFieldValue(string name, OdCmEntityColor value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("R: {0} G: {1} B: {2}", value.red(), value.green(), value.blue()));

	}
	public void writeFieldValue(string name, OdDgLineStyleInfo value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("Modifiers: {0:X} Scale: {1} Shift: {2} Start width: {3} End width: {4}",
			value.getModifiers(), value.getScale(), value.getShift(), value.getStartWidth(), value.getEndWidth()));
	}
	public void writeFieldValue(string name, UInt64 value)
	{
		dumpFieldName(name);
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValueHex(string name, UInt64 value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:X}", value));
	}
	public void writeFieldValue(string name, byte value)
	{
		dumpFieldName(name);
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValueHex(string name, byte value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:X}", value));
	}
	public void writeFieldValue(string name, OdDgLightColor value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("R: {0} G: {1} B: {2} Intensity: {3}",
			value.getRed(), value.getGreen(), value.getBlue(), value.getIntensityScale()));
	}
	public void writeFieldValue(string name, bool value)
	{
		dumpFieldName(name);
		dumpFieldValue(value ? "true" : "false");
	}
	public void writeFieldValue(string name, OdAngleCoordinate value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0}° {1}' {2}\"",
			value.getDegrees(), value.getMinutes(), value.getSeconds()));
	}
	public void writeFieldValue(string name, Int16 value)
	{
		dumpFieldName(name);
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValue(string name, Int32 value)
	{
		dumpFieldName(name);
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValue(string name, Int64 value)
	{
		dumpFieldName(name);
		dumpFieldValue(value.ToString());
	}
	public void writeFieldValue(string name, OdDgModel.WorkingUnit value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgModel.WorkingUnit.kWuUnitOfResolution: val = "unit of resolution"; break;
			case OdDgModel.WorkingUnit.kWuStorageUnit: val = "storage unit"; break;
			case OdDgModel.WorkingUnit.kWuWorldUnit: val = "world unit"; break;
			case OdDgModel.WorkingUnit.kWuMasterUnit: val = "master unit"; break;
			case OdDgModel.WorkingUnit.kWuSubUnit: val = "sub unit"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, TextJustification value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case TextJustification.kLeftTop: val = "left top"; break;
			case TextJustification.kLeftCenter: val = "left center"; break;
			case TextJustification.kLeftBottom: val = "left bottom"; break;
			case TextJustification.kLeftMarginTop: val = "left margin top"; break;
			case TextJustification.kLeftMarginCenter: val = "left margin center"; break;
			case TextJustification.kLeftMarginBottom: val = "left margin bottom"; break;
			case TextJustification.kCenterTop: val = "center top"; break;
			case TextJustification.kCenterCenter: val = "center center"; break;
			case TextJustification.kCenterBottom: val = "center bottom"; break;
			case TextJustification.kRightMarginTop: val = "right margin top"; break;
			case TextJustification.kRightMarginCenter: val = "right margin center"; break;
			case TextJustification.kRightMarginBottom: val = "right margin bottom"; break;
			case TextJustification.kRightTop: val = "right top"; break;
			case TextJustification.kRightCenter: val = "right center"; break;
			case TextJustification.kRightBottom: val = "right bottom"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdGeQuaternion value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:0.###}; {1:0.###}; {2:0.###}; {3:0.###}", value.w, value.x, value.y, value.z));
	}
	//public void writeFieldValue( string name, TextAttributes value );
	public void writeFieldValue(string name, OdDgGraphicsElement.Class value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgGraphicsElement.Class.kClassPrimary: val = "Primary"; break;
			case OdDgGraphicsElement.Class.kClassPatternComponent: val = "Pattern component"; break;
			case OdDgGraphicsElement.Class.kClassConstruction: val = "Construction"; break;
			case OdDgGraphicsElement.Class.kClassDimension: val = "Dimension"; break;
			case OdDgGraphicsElement.Class.kClassPrimaryRule: val = "Primary rule"; break;
			case OdDgGraphicsElement.Class.kClassLinearPatterned: val = "Linear patterned"; break;
			case OdDgGraphicsElement.Class.kClassConstructionRule: val = "Construction rule"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdGeMatrix2d value)
	{
		dumpFieldName(name);
		dumpFieldValue(String.Format("{0:0.###}; {1:0.###}; {2:0.###}; {3:0.###}", value[0, 0], value[1, 0], value[0, 1], value[1, 1]));
	}
	//public void writeFieldValue( string name, OdDgDimension.ToolType value );
	public void writeFieldValue(string name, OdDgDimPoint value)
	{
		dumpFieldName(name + ":");
		writeFieldValue("  Point", value.getPoint());
		writeFieldValue("  Offset to dimension line", value.getOffsetToDimLine());
		writeFieldValue("  Offset Y", value.getOffsetY());
		writeFieldValue("  Text alignment", value.getJustification());
		dumpFieldName("  Flags:");
		writeFieldValue("    Associative", value.getAssociativeFlag());
		writeFieldValue("    Relative", value.getRelative() != 0);
		writeFieldValue("    WitnessControlLocal", value.getWitnessControlLocalFlag());
		writeFieldValue("    NoWitnessLine", value.getNoWitnessLineFlag());
		writeFieldValue("    UseAltSymbology", value.getUseAltSymbologyFlag());

		if (value.getPrimaryTextFlag())
		{
			writeFieldValue("  Primary text", value.getPrimaryText());
		}

		if (value.getPrimaryTopToleranceTextFlag())
		{
			writeFieldValue("  Primary Top text", value.getPrimaryTopToleranceText());
		}

		if (value.getPrimaryBottomToleranceTextFlag())
		{
			writeFieldValue("  Primary Bottom text", value.getPrimaryBottomToleranceText());
		}

		if (value.getSecondaryTextFlag())
		{
			writeFieldValue("  Secondary text", value.getSecondaryText());
		}

		if (value.getSecondaryTopToleranceTextFlag())
		{
			writeFieldValue("  Secondary Top text", value.getSecondaryTopToleranceText());
		}

		if (value.getSecondaryBottomToleranceTextFlag())
		{
			writeFieldValue("  Secondary Bottom text", value.getSecondaryBottomToleranceText());
		}
	}
	public void writeFieldValue(string name, OdDgDimTextInfo value)
	{
		dumpFieldName(name + ":");
		writeFieldValue("  Width", value.getWidth());
		writeFieldValue("  Height", value.getHeight());
		writeFieldValue("  Font ID", value.getFontEntryId());
		writeFieldValue("  Color", value.getColorIndex());
		writeFieldValue("  Weight", value.getWeight());

		switch (value.getStackedFractionType())
		{
			case OdDgDimTextInfo.StackedFractionType.kFractionFromText:
				{
					writeFieldValue("  Stacked Fraction Type", "kFractionFromText");
				}
				break;

			case OdDgDimTextInfo.StackedFractionType.kFractionHorizontal:
				{
					writeFieldValue("  Stacked Fraction Type", "kFractionHorizontal");
				}
				break;

			case OdDgDimTextInfo.StackedFractionType.kFractionDiagonal:
				{
					writeFieldValue("  Stacked Fraction Type", "kFractionDiagonal");
				}
				break;
		}

		switch (value.getStackFractAlignment())
		{
			case OdDgDimTextInfo.StackFractionAlignment.kFractAlignmentTop:
				{
					writeFieldValue("  Stacked Fraction Alignment", "kFractAlignmentTop");
				}
				break;

			case OdDgDimTextInfo.StackFractionAlignment.kFractAlignmentCenter:
				{
					writeFieldValue("  Stacked Fraction Alignment", "kFractAlignmentCenter");
				}
				break;

			case OdDgDimTextInfo.StackFractionAlignment.kFractAlignmentBottom:
				{
					writeFieldValue("  Stacked Fraction Alignment", "kFractAlignmentBottom");
				}
				break;
		}

		dumpFieldName("  Text flags:");
		writeFieldValue("    Use text color", value.getUseColorFlag());
		writeFieldValue("    Use weight ", value.getUseWeightFlag());
		writeFieldValue("    Show primary master units ", !value.getPrimaryNoMasterUnitsFlag());
		writeFieldValue("    Has primary alt format ", value.getHasAltFormatFlag());
		writeFieldValue("    Show secondary master units ", !value.getSecNoMasterUnitsFlag());
		writeFieldValue("    Has secondary alt format ", value.getHasSecAltFormatFlag());
	}
	public void writeFieldValue(string name, OdDgDimTextFormat value)
	{
		dumpFieldName(name + ":");

		writeFieldValue("  Primary accuracy", value.getPrimaryAccuracy());
		writeFieldValue("  Secondary accuracy", value.getSecondaryAccuracy());

		switch (value.getAngleMode())
		{
			case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_D:
				{
					writeFieldValue("  Angle display mode", "kAngle_D");
				}
				break;

			case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_DM:
				{
					writeFieldValue("  Angle display mode", "kAngle_DM");
				}
				break;

			case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_DMS:
				{
					writeFieldValue("  Angle display mode", "kAngle_DMS");
				}
				break;

			case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_C:
				{
					writeFieldValue("  Angle display mode", "kAngle_C");
				}
				break;

			case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_Radian:
				{
					writeFieldValue("  Angle display mode", "kAngle_Radian");
				}
				break;
		}

		dumpFieldName("  Text Format Flags:");
		writeFieldValue("    AngleMeasure", value.getAngleMeasureFlag());
		writeFieldValue("    AngleFormat", value.getAngleFormatFlag());
		writeFieldValue("    PrimarySubUnits", value.getPrimarySubUnitsFlag());
		writeFieldValue("    PrimaryLabels", value.getPrimaryLabelsFlag());
		writeFieldValue("    PrimaryDelimiter", value.getPrimaryDelimiterFlag());
		writeFieldValue("    DecimalComma", value.getDecimalCommaFlag());
		writeFieldValue("    SuperScriptLSD", value.getSuperScriptLSDFlag());
		writeFieldValue("    RoundLSD", value.getRoundLSDFlag());
		writeFieldValue("    OmitLeadDelimiter", value.getOmitLeadDelimiterFlag());
		writeFieldValue("    LocalFileUnits", value.getLocalFileUnitsFlag());
		writeFieldValue("    UnusedDeprecated", value.getUnusedDeprecatedFlag());
		writeFieldValue("    ThousandSeparator", value.getThousandSepFlag());
		writeFieldValue("    MetricSpace", value.getMetricSpaceFlag());
		writeFieldValue("    SecondarySubUnits", value.getSecondarySubUnitsFlag());
		writeFieldValue("    SecondaryLabels", value.getSecondaryLabelsFlag());
		writeFieldValue("    SecondaryDelimiter", value.getSecondaryDelimiterFlag());
		writeFieldValue("    Radians", value.getRadiansFlag());
		writeFieldValue("    Show primary master units if zero", value.getPriAllowZeroMastFlag());
		writeFieldValue("    Show secondary master units if zero", value.getSecAllowZeroMastFlag());
		writeFieldValue("    Show primary sub units if zero", !value.getPriSubForbidZeroMastFlag());
		writeFieldValue("    Show secondary sub units if zero", !value.getSecSubForbidZeroMastFlag());
		writeFieldValue("    HideAngleSeconds", !value.getHideAngleSecondsFlag());
		writeFieldValue("    SkipNonStackedFractionSpace", !value.getSkipNonStackedFractionSpaceFlag());
	}
	public void writeFieldValue(string name, OdDgDimTextFormat.Accuracy value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgDimTextFormat.Accuracy.kAccuracyNone: val = "1 digit"; break;

			case OdDgDimTextFormat.Accuracy.kDecimal1: val = "Decimal, 2 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal2: val = "Decimal, 3 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal3: val = "Decimal, 4 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal4: val = "Decimal, 5 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal5: val = "Decimal, 6 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal6: val = "Decimal, 7 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal7: val = "Decimal, 8 digit"; break;
			case OdDgDimTextFormat.Accuracy.kDecimal8: val = "Decimal, 9 digit"; break;

			case OdDgDimTextFormat.Accuracy.kFractional2: val = "Fractional, 2-th"; break;
			case OdDgDimTextFormat.Accuracy.kFractional4: val = "Fractional, 4-th"; break;
			case OdDgDimTextFormat.Accuracy.kFractional8: val = "Fractional, 8-th"; break;
			case OdDgDimTextFormat.Accuracy.kFractional16: val = "Fractional, 16-th"; break;
			case OdDgDimTextFormat.Accuracy.kFractional32: val = "Fractional, 32-th"; break;
			case OdDgDimTextFormat.Accuracy.kFractional64: val = "Fractional, 64-th"; break;

			case OdDgDimTextFormat.Accuracy.kExponential1: val = "Exponential, 1 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential2: val = "Exponential, 2 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential3: val = "Exponential, 3 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential4: val = "Exponential, 4 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential5: val = "Exponential, 5 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential6: val = "Exponential, 6 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential7: val = "Exponential, 7 digit for mantissa"; break;
			case OdDgDimTextFormat.Accuracy.kExponential8: val = "Exponential, 8 digit for mantissa"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdDgDimGeometry value)
	{
		dumpFieldName(name + ":");

		writeFieldValue("  Witness line offset", value.getWitnessLineOffset());
		writeFieldValue("  Witness line extend", value.getWitnessLineExtend());
		writeFieldValue("  Text lift", value.getTextLift());
		writeFieldValue("  Text margin", value.getTextMargin());
		writeFieldValue("  Terminator width", value.getTerminatorWidth());
		writeFieldValue("  Terminator height", value.getTerminatorHeight());
		writeFieldValue("  Stack offset", value.getStackOffset());
		writeFieldValue("  Center size", value.getCenterSize());

		if (value.getUseMargin())
			writeFieldValue("  Min leader", value.getMargin());
		else
			writeFieldValue("  Min leader", value.getTerminatorWidth() * 2.0);
	}
	public void writeFieldValue(string name, OdDgDimOption value)
	{
		dumpFieldName(name + ":");

		if (value != null)
		{
			switch (value.getType())
			{
				case OdDgDimOption.Type.kNone:
					{
						writeFieldValue("  Type", "kNone");
					}
					break;

				case OdDgDimOption.Type.kTolerance:
					{
						OdDgDimOptionTolerance pTolerOptions = (OdDgDimOptionTolerance)value;
						writeFieldValue("", pTolerOptions);
					}
					break;

				case OdDgDimOption.Type.kTerminators:
					{
						OdDgDimOptionTerminators pTermOptions = (OdDgDimOptionTerminators)value;
						writeFieldValue("", pTermOptions);
					}
					break;

				case OdDgDimOption.Type.kPrefixSymbol:
					{
						OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
						writeFieldValue("  Type", "kPrefixSymbol");
						writeFieldValue("", pSymbolOptions);
					}
					break;

				case OdDgDimOption.Type.kSuffixSymbol:
					{
						OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
						writeFieldValue("  Type", "kSuffixSymbol");
						writeFieldValue("", pSymbolOptions);
					}
					break;

				case OdDgDimOption.Type.kDiameterSymbol:
					{
						OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
						writeFieldValue("  Type", "kDiameterSymbol");
						writeFieldValue("", pSymbolOptions);
					}
					break;

				case OdDgDimOption.Type.kPrefixSuffix:
					{
						OdDgDimOptionPrefixSuffix pPrefixSuffixOptions = (OdDgDimOptionPrefixSuffix)value;
						writeFieldValue("", pPrefixSuffixOptions);
					}
					break;

				case OdDgDimOption.Type.kPrimaryUnits:
					{
						OdDgDimOptionUnits pUnitsOptions = (OdDgDimOptionUnits)value;
						writeFieldValue("  Type", "kPrimaryUnits");
						writeFieldValue("", pUnitsOptions);
					}
					break;

				case OdDgDimOption.Type.kSecondaryUnits:
					{
						OdDgDimOptionUnits pUnitsOptions = (OdDgDimOptionUnits)value;
						writeFieldValue("  Type", "kSecondaryUnits");
						writeFieldValue("", pUnitsOptions);
					}
					break;

				case OdDgDimOption.Type.kTerminatorSymbology:
					{
						OdDgDimOptionTerminatorSymbology pTermSymbolOptions = (OdDgDimOptionTerminatorSymbology)value;
						writeFieldValue("", pTermSymbolOptions);
					}
					break;

				case OdDgDimOption.Type.kView:
					{
						OdDgDimOptionView pViewOptions = (OdDgDimOptionView)value;
						writeFieldValue("  Type", "kView");

						if (pViewOptions != null)
							writeFieldValue("  Rotation", pViewOptions.getQuaternion());
					}
					break;

				case OdDgDimOption.Type.kAlternatePrimaryUnits:
					{
						OdDgDimOptionAltFormat pAltOptions = (OdDgDimOptionAltFormat)value;
						writeFieldValue("  Type", "kAlternativePrimaryUnits");
						writeFieldValue("", pAltOptions);
					}
					break;

				case OdDgDimOption.Type.kOffset:
					{
						OdDgDimOptionOffset pOffsetOptions = (OdDgDimOptionOffset)value;
						writeFieldValue("", pOffsetOptions);
					}
					break;

				case OdDgDimOption.Type.kAlternateSecondaryUnits:
					{
						OdDgDimOptionAltFormat pAltOptions = (OdDgDimOptionAltFormat)value;
						writeFieldValue("  Type", "kAlternativeSecondaryUnits");
						writeFieldValue("", pAltOptions);
					}
					break;

				case OdDgDimOption.Type.kAlternatePrefixSymbol:
					{
						OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
						writeFieldValue("  Type", "kAlternatePrefixSymbol");
						writeFieldValue("", pSymbolOptions);
					}
					break;

				case OdDgDimOption.Type.kAlternateSuffixSymbol:
					{
						OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
						writeFieldValue("  Type", "kAlternateSuffixSymbol");
						writeFieldValue("", pSymbolOptions);
					}
					break;

				case OdDgDimOption.Type.kProxyCell:
					{
						OdDgDimOptionProxyCell pCellOptions = (OdDgDimOptionProxyCell)value;
						writeFieldValue("", pCellOptions);
					} break;
			}
		}
		else
		{
			writeFieldValue("  Type", "[value unknown]");
		}
	}
	public void writeFieldValue(string name, OdDgDimOptionSymbol value)
	{
		if (value == null)
			return;

		writeFieldValue("  Font entry Id", value.getSymbolFont());
		writeFieldValue("  Symbol code", value.getSymbolChar());
	}
	public void writeFieldValue(string name, OdDgDimOptionTerminatorSymbology value)
	{
		writeFieldValue("  Type", "kTerminatorSymbology");

		if (value == null)
			return;

		writeFieldValue("  Use Line type", value.getStyleFlag());
		writeFieldValue("  Use Line weight", value.getWeightFlag());
		writeFieldValue("  Use Color", value.getColorFlag());

		if (value.getStyleFlag())
			writeFieldValue("  Line Type entry Id", value.getStyle());

		if (value.getWeightFlag())
			writeFieldValue("  Line Weight", value.getWeight());

		if (value.getColorFlag())
			writeFieldValue("  Color", value.getColor());
	}
	public void writeFieldValue(string name, OdDgDimOptionTolerance value)
	{
		writeFieldValue("  Type", "kTolerance");

		if (value == null)
			return;

		writeFieldValue("  Upper value", value.getToleranceUpper());
		writeFieldValue("  Lower value", value.getToleranceLower());
		writeFieldValue("  Stack if equal", value.getStackEqualFlag());
		writeFieldValue("  Show sign for zero", value.getShowSignForZeroFlag());
		writeFieldValue("  Left margin", value.getToleranceHorizSep());
		writeFieldValue("  Separator margin", value.getToleranceVertSep());
		writeFieldValue("  Font entry Id", value.getFontEntryId());
		writeFieldValue("  Text Width", value.getToleranceTextWidth());
		writeFieldValue("  Text Height", value.getToleranceTextHeight());

		if (value.getTolerancePlusMinusSymbol() != 0)
			writeFieldValue("  Plus/Minus symbol", value.getTolerancePlusMinusSymbol());

		if (value.getTolerancePrefixSymbol() != 0)
			writeFieldValue("  Prefix symbol", value.getTolerancePrefixSymbol());

		if (value.getToleranceSuffixSymbol() != 0)
			writeFieldValue("  Suffix symbol", value.getToleranceSuffixSymbol());

		writeFieldValue("  Stack align", value.getStackAlign());
	}
	public void writeFieldValue(string name, OdDgDimOptionTerminators value)
	{
		writeFieldValue("  Type", "kTerminators");

		if (value == null)
			return;

		if (value.getArrowTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault)
		{
			writeFieldValue("  Arrow style", "kTermDefault");
		}
		else if (value.getArrowTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol)
		{
			writeFieldValue("  Arrow style", "kTermSymbol");
			writeFieldValue("  Arrow Font entry Id", value.getArrowFontID());
			writeFieldValue("  Arrow Symbol code", value.getArrowSymbol());
		}
		else if (value.getArrowTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell)
		{
			writeFieldValue("  Arrow style", "kTermCell");
			writeFieldValue("  Arrow Cell Id", value.getArrowCellID());
		}
		else
		{
			writeFieldValue("  Arrow style", "kTermScaledCell");
			writeFieldValue("  Arrow Cell Id", value.getArrowCellID());
			writeFieldValue("  Arrow Cell scale", value.getSharedCellScale());
		}

		if (value.getStrokeTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault)
		{
			writeFieldValue("  Stroke style", "kTermDefault");
		}
		else if (value.getStrokeTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol)
		{
			writeFieldValue("  Stroke style", "kTermSymbol");
			writeFieldValue("  Stroke Font entry Id", value.getStrokeFontID());
			writeFieldValue("  Stroke Symbol code", value.getStrokeSymbol());
		}
		else if (value.getStrokeTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell)
		{
			writeFieldValue("  Stroke style", "kTermCell");
			writeFieldValue("  Stroke Cell Id", value.getStrokeCellID());
		}
		else
		{
			writeFieldValue("  Stroke style", "kTermScaledCell");
			writeFieldValue("  Stroke Cell Id", value.getStrokeCellID());
			writeFieldValue("  Stroke Cell scale", value.getSharedCellScale());
		}

		if (value.getOriginTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault)
		{
			writeFieldValue("  Origin style", "kTermDefault");
		}
		else if (value.getOriginTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol)
		{
			writeFieldValue("  Origin style", "kTermSymbol");
			writeFieldValue("  Origin Font entry Id", value.getOriginFontID());
			writeFieldValue("  Origin Symbol code", value.getOriginSymbol());
		}
		else if (value.getOriginTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell)
		{
			writeFieldValue("  Origin style", "kTermCell");
			writeFieldValue("  Origin Cell Id", value.getOriginCellID());
		}
		else
		{
			writeFieldValue("  Origin style", "kTermScaledCell");
			writeFieldValue("  Origin Cell Id", value.getOriginCellID());
			writeFieldValue("  Origin Cell scale", value.getSharedCellScale());
		}

		if (value.getDotTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault)
		{
			writeFieldValue("  Dot style", "kTermDefault");
		}
		else if (value.getDotTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol)
		{
			writeFieldValue("  Dot style", "kTermSymbol");
			writeFieldValue("  Dot Font entry Id", value.getDotFontID());
			writeFieldValue("  Dot Symbol code", value.getDotSymbol());
		}
		else if (value.getDotTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell)
		{
			writeFieldValue("  Dot style", "kTermCell");
			writeFieldValue("  Dot Cell Id", value.getDotCellID());
		}
		else
		{
			writeFieldValue("  Dot style", "kTermScaledCell");
			writeFieldValue("  Dot Cell Id", value.getDotCellID());
			writeFieldValue("  Dot Cell scale", value.getSharedCellScale());
		}
	}
	public void writeFieldValue(string name, OdDgDimOptionPrefixSuffix value)
	{
		writeFieldValue("  Type", "kPrefixSuffix");

		if (value == null)
			return;

		if (value.getMainPrefix() != 0)
		{
			writeFieldValue("  Main prefix", value.getMainPrefix());
		}

		if (value.getMainSuffix() != 0)
		{
			writeFieldValue("  Main suffix", value.getMainSuffix());
		}

		if (value.getUpperPrefix() != 0)
		{
			writeFieldValue("  Upper prefix", value.getUpperPrefix());
		}

		if (value.getUpperSuffix() != 0)
		{
			writeFieldValue("  Upper suffix", value.getUpperSuffix());
		}

		if (value.getLowerPrefix() != 0)
		{
			writeFieldValue("  Lower prefix", value.getLowerPrefix());
		}

		if (value.getLowerSuffix() != 0)
		{
			writeFieldValue("  Lower suffix", value.getLowerSuffix());
		}
	}
	public void writeFieldValue(string name, OdDgModel.UnitSystem value)
	{
		if (value == OdDgModel.UnitSystem.kCustom)
		{
			writeFieldValue(name, "kCustom");
		}
		else if (value == OdDgModel.UnitSystem.kMetric)
		{
			writeFieldValue(name, "kMetric");
		}
		else if (value == OdDgModel.UnitSystem.kEnglish)
		{
			writeFieldValue(name, "kEnglish");
		}
		else
		{
			writeFieldValue(name, "Unknown");
		}
	}
	public void writeFieldValue(string name, OdDgModel.UnitBase value)
	{
		if (value == OdDgModel.UnitBase.kNone)
		{
			writeFieldValue(name, "kNone");
		}
		else if (value == OdDgModel.UnitBase.kMeter)
		{
			writeFieldValue(name, "kMeter");
		}
		else
		{
			writeFieldValue(name, "Unknown");
		}
	}
	public void writeFieldValue(string name, OdDgDimOptionUnits value)
	{
		if (value == null)
			return;

		OdDgModel.UnitDescription descr = new OdDgModel.UnitDescription();
		value.getMasterUnit(descr);
		dumpFieldName("  Master units:");
		writeFieldValue("    Unit base", descr.m_base);
		writeFieldValue("    Unit system", descr.m_system);
		writeFieldValue("    Denominator", descr.m_denominator);
		writeFieldValue("    Numerator", descr.m_numerator);
		writeFieldValue("    Name", descr.m_name);
		value.getSubUnit(descr);
		dumpFieldName("  Sub units:");
		writeFieldValue("    Unit base", descr.m_base);
		writeFieldValue("    Unit system", descr.m_system);
		writeFieldValue("    Denominator", descr.m_denominator);
		writeFieldValue("    Numerator", descr.m_numerator);
		writeFieldValue("    Name", descr.m_name);
	}
	public void writeFieldValue(string name, OdDgDimOptionAltFormat value)
	{
		if (value == null)
			return;

		writeFieldValue("  Accuracy", value.getAccuracy());
		writeFieldValue("  Show sub units", value.getSubUnits());
		writeFieldValue("  Show unit labels", value.getLabel());
		writeFieldValue("  Show delimiter", value.getDelimiter());
		writeFieldValue("  Show sub units only", value.getNoMasterUnits());
		writeFieldValue("  Allow zero master units", value.getAllowZeroMasterUnits());

		if (value.getMoreThanThreshold())
		{
			if (value.getEqualToThreshold())
			{
				writeFieldValue("  Condition", ">=");
			}
			else
			{
				writeFieldValue("  Condition", ">");
			}
		}
		else
		{
			if (value.getEqualToThreshold())
			{
				writeFieldValue("  Condition", "<=");
			}
			else
			{
				writeFieldValue("  Condition", "<");
			}
		}

		writeFieldValue("  Threshold", value.getThreshold());
	}
	public void writeFieldValue(string name, OdDgDimOptionOffset value)
	{
		writeFieldValue("  Type", "kOffset");

		if (value == null)
			return;

		writeFieldValue("  Terminator", value.getTerminator());
		writeFieldValue("  Chain type", value.getChainType());
		writeFieldValue("  Elbow", value.getElbowFlag());
		writeFieldValue("  Alignment", value.getAlignment());
		writeFieldValue("  No dock on dim line", value.getNoDockOnDimLineFlag());
	}
	public void writeFieldValue(string name, OdDgDimOptionProxyCell value)
	{
		writeFieldValue("  Type", "kProxyCell");

		if (value == null)
			return;

		writeFieldValue("  Origin", value.getOrigin());
		writeFieldValue("  Rotation Matrix", value.getRotScale());
		writeFieldValue("  Check Sum", value.getCheckSum());
	}
	public void writeFieldValue(string name, OdDgMultilineSymbology value)
	{
		dumpFieldName(name + ":");

		writeFieldValue("Style", value.getLineStyleEntryId());
		writeFieldValue("Weight", value.getLineWeight());
		writeFieldValue("Color", value.getColorIndex());
		writeFieldValue("Use style", value.getUseStyleFlag());
		writeFieldValue("Use weight", value.getUseWeightFlag());
		writeFieldValue("Use color", value.getUseColorFlag());
		writeFieldValue("Use class", value.getUseClassFlag());
		writeFieldValue("Inside arc", value.getCapInArcFlag());
		writeFieldValue("Outside arc", value.getCapOutArcFlag());
		writeFieldValue("Cap line", value.getCapLineFlag());
		writeFieldValue("Custom style", value.getCustomStyleFlag());
		writeFieldValue("Cap color from segment", value.getCapColorFromSegmentFlag());
		writeFieldValue("Construction class", value.getConstructionClassFlag());
	}
	public void writeFieldValue(string name, OdDgMultilinePoint value)
	{
		dumpFieldName(name + ":");

		{
			OdGePoint3d point = new OdGePoint3d();
			value.getPoint(point);
			writeFieldValue("Point", point);
		}

		{
			UInt32 j = value.getBreaksCount();
			writeFieldValue("Number of breaks", j);
			for (UInt32 i = 0; i < j; i++)
			{
				OdDgMultilineBreak break_ = new OdDgMultilineBreak();
				value.getBreak(i, break_);
				writeFieldValue("Break " + i.ToString(), break_);
			}
		}
	}
	public void writeFieldValue(string name, OdDgMultilineBreak value)
	{
		dumpFieldName(name + ":");

		writeFieldValue("Lines mask", value.getLinesMask());
		writeFieldValue("Offset", value.getOffset());
		writeFieldValue("Length", value.getLength());

		{
			String flagValue = "";

			switch (value.getFlags())
			{
				case OdDgMultilineBreak.Flags.kStandardByDistance: flagValue = "Standard by distance"; break;
				case OdDgMultilineBreak.Flags.kFromJoint: flagValue = "from joint"; break;
				case OdDgMultilineBreak.Flags.kToJoint: flagValue = "to joint"; break;
			}

			writeFieldValue("Flag", flagValue);
		}
	}
	public void writeFieldValue(string name, OdDgMultilineProfile value)
	{
		dumpFieldName(name + ":");

		writeFieldValue("Distance", value.getDistance());

		{
			OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

			value.getSymbology(symbology);
			writeFieldValue("Symbology", symbology);
		}

	}
	//public void writeFieldValue(string name, OdDgSurface.Type value);
	public void writeFieldValue(string name, OdDgRaster.RasterFormat value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgRaster.RasterFormat.kBitmap: val = "Bitmap raster"; break;
			case OdDgRaster.RasterFormat.kByteData: val = "Byte data raster"; break;
			case OdDgRaster.RasterFormat.kBinaryRLE: val = "Binary RLE raster"; break;
			case OdDgRaster.RasterFormat.kByteRLE: val = "Byte RLE raster"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdDgTagDefinition.Type value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgTagDefinition.Type.kChar: val = "Char"; break;
			case OdDgTagDefinition.Type.kInt16: val = "Short int"; break;
			case OdDgTagDefinition.Type.kInt32: val = "Long int"; break;
			case OdDgTagDefinition.Type.kDouble: val = "Double"; break;
			case OdDgTagDefinition.Type.kBinary: val = "Binary"; break;
		}
		dumpFieldValue(val);

	}
	public void writeFieldValue(string name, TextDirection value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case TextDirection.kHorizontal: val = "Horizontal"; break;
			case TextDirection.kVertical: val = "Vertical"; break;
			case TextDirection.kJapanese: val = "Japanese"; break;
			case TextDirection.kRightToLeft: val = "Right-to-left"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdGeMatrix3d value)
	{
		dumpFieldName(name);

		dumpFieldValue(String.Format("{0:G4}; {1:G4}; {2:G4}; {3:G4}; {4:G4}; {5:G4}; {6:G4}; {7:G4}; {8:G4}",
			value[0, 0], value[1, 0], value[2, 0],
			value[0, 1], value[1, 1], value[2, 1],
			value[0, 2], value[1, 2], value[2, 2]));

	}
	public void writeFieldValue(string name, OdGsDCRect value)
	{
		dumpFieldName(name);

		dumpFieldValue(String.Format("( 0x{0:X}; 0x{1:X} ) - ( 0x{2:X}; 0x{3:X} )\n", value.m_min.x, value.m_min.y, value.m_max.x, value.m_max.y));

	}
	public void writeFieldValue(string name, OdDgElementId value)
	{
		writeFieldValue(name, value.getHandle().ascii());
	}
	public void writeFieldValue(string name, object value)
	{
		writeFieldValue(name, value.ToString());
	}
	public void writeFieldValue(string name, OdGeExtents2d value)
	{
		dumpFieldName(name);

		OdGePoint2d min = value.minPoint(), max = value.maxPoint();

		dumpFieldValue(String.Format("Low point: {0:G4}, {1:G4};", min.x, min.y)
								 + String.Format("High point: {0:G4}, {1:G4}", max.x, max.y));
	}
	public void writeFieldValue(string name, OdGeExtents3d value)
	{
		dumpFieldName(name);

		OdGePoint3d min = value.minPoint(), max = value.maxPoint();

		dumpFieldValue(String.Format("Low point: {0:G4}, {1:G4}, {2:G4};", min.x, min.y, min.z)
								 + String.Format("High point: {0:G4}, {1:G4}, {2:G4}; ", max.x, max.y, max.z));
	}
	public void writeFieldValue(string name, LineSpacingType value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case LineSpacingType.kExact: val = "Exact"; break;
			case LineSpacingType.kAutomatic: val = "Automatic"; break;
			case LineSpacingType.kFromLineTop: val = "FromLineTop"; break;
			case LineSpacingType.kAtLeast: val = "AtLeast"; break;
		}
		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, LineStyleType value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case LineStyleType.kLsTypePointSymbol:
				val = "kPointSymbol";
				break;
			case LineStyleType.kLsTypeCompound:
				val = "kCompound";
				break;
			case LineStyleType.kLsTypeLineCode:
				val = "kLineCode";
				break;
			case LineStyleType.kLsTypeLinePoint:
				val = "kLinePoint";
				break;
		}

		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, LineStyleUnitsType value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case LineStyleUnitsType.kLsMasterUnits:
				val = "kMasterUnits";
				break;
			case LineStyleUnitsType.kLsUORS:
				val = "kUORs";
				break;
			case LineStyleUnitsType.kLsDeviceUnits:
				val = "kDeviceUnits";
				break;
		}

		dumpFieldValue(val);

	}
	public void writeFieldValue(string name, OdDgLineStyleResource.OdLsResourceType value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgLineStyleResource.OdLsResourceType.kLsUnknownRes:
				val = "kUnknownRes";
				break;
			case OdDgLineStyleResource.OdLsResourceType.kLsPointSymbolResV7:
				val = "kPointSymbolV7Res";
				break;
			case OdDgLineStyleResource.OdLsResourceType.kLsPointSymbolRes:
				val = "kPointSymbolRes";
				break;
			case OdDgLineStyleResource.OdLsResourceType.kLsCompoundRes:
				val = "kCompoundRes";
				break;
			case OdDgLineStyleResource.OdLsResourceType.kLsLineCodeRes:
				val = "kLineCodeRes";
				break;
			case OdDgLineStyleResource.OdLsResourceType.kLsLinePointRes:
				val = "kLinePointRes";
				break;
			case OdDgLineStyleResource.OdLsResourceType.kLsInternalRes:
				val = "kInternalRes";
				break;
		}

		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdDgLineCodeResourceStrokeData.OdLsStrokeWidthMode value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgLineCodeResourceStrokeData.OdLsStrokeWidthMode.kLsNoWidth:
				val = "kNoWidth";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeWidthMode.kLsWidthLeft:
				val = "kLeftWidth";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeWidthMode.kLsWidthRight:
				val = "kRightWidth";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeWidthMode.kLsWidthFull:
				val = "kFullWidth";
				break;
		}

		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsClosed:
				val = "kCapsClosed";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsOpen:
				val = "kCapsOpen";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsExtended:
				val = "kCapsExtended";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsHexagon:
				val = "kCapsHexagon";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsOctagon:
				val = "kCapsOctagon";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsDecagon:
				val = "kCapsDecagon";
				break;
			case OdDgLineCodeResourceStrokeData.OdLsStrokeCapsType.kLsCapsArc:
				val = "kCapsArc";
				break;
		}

		dumpFieldValue(val);

	}
	public void writeFieldValue(string name, OdDgLinePointResourceSymInfo.OdLsSymbolPosOnStroke value)
	{
		dumpFieldName(name);

		String val = "";
		switch (value)
		{
			case OdDgLinePointResourceSymInfo.OdLsSymbolPosOnStroke.kLsNoSymbol:
				val = "kNoSymbol";
				break;
			case OdDgLinePointResourceSymInfo.OdLsSymbolPosOnStroke.kLsAtOriginOfStroke:
				val = "kAtOriginOfStroke";
				break;
			case OdDgLinePointResourceSymInfo.OdLsSymbolPosOnStroke.kLsAtEndOfStroke:
				val = "kAtEndOfStroke";
				break;
			case OdDgLinePointResourceSymInfo.OdLsSymbolPosOnStroke.kLsAtCenterOfStroke:
				val = "kAtCenterOfStroke";
				break;
		}

		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdDgDimLabelLine.LabelLineDimensionMode value)
	{
		dumpFieldName(name);
		String val = "";

		switch (value)
		{
			case OdDgDimLabelLine.LabelLineDimensionMode.kAngleLength:
				val = "Angle/Length"; break;
			case OdDgDimLabelLine.LabelLineDimensionMode.kLengthAbove:
				val = "Length above"; break;
			case OdDgDimLabelLine.LabelLineDimensionMode.kAngleAbove:
				val = "Angle above"; break;
			case OdDgDimLabelLine.LabelLineDimensionMode.kLengthBelow:
				val = "Length below"; break;
			case OdDgDimLabelLine.LabelLineDimensionMode.kAngleBelow:
				val = "Angle below"; break;
			case OdDgDimLabelLine.LabelLineDimensionMode.kLengthAngleAbove:
				val = "Length Angle above"; break;
			case OdDgDimLabelLine.LabelLineDimensionMode.kLengthAngleBelow:
				val = "Length Angle below"; break;
			default:
				val = "Length/Angle"; break;
		}

		dumpFieldValue(val);
	}
	public void writeFieldValue(string name, OdDgDimTextInfo.FitOptions value)
	{
		dumpFieldName(name);

		string strVal = "";

		switch (value)
		{
			case OdDgDimTextInfo.FitOptions.kTermMovesFirst: strVal = "kTermMovesFirst"; break;
			case OdDgDimTextInfo.FitOptions.kTermReversed: strVal = "kTermReversed"; break;
			case OdDgDimTextInfo.FitOptions.kTermInside: strVal = "kTermInside"; break;
			case OdDgDimTextInfo.FitOptions.kTermOutside: strVal = "kTermOutside"; break;
			case OdDgDimTextInfo.FitOptions.kTextInside: strVal = "kTextInside"; break;
			case OdDgDimTextInfo.FitOptions.kTextMovesFirst: strVal = "kTextMovesFirst"; break;
			case OdDgDimTextInfo.FitOptions.kBothMoves: strVal = "kBothMoves"; break;
			case OdDgDimTextInfo.FitOptions.kSmallestMoves: strVal = "kSmallestMoves"; break;
		}

		dumpFieldValue(strVal);
	}
	public void writeFieldValue(string name, OdDgDimension.PlacementAlignment value)
	{
		string strAlign = "";

		switch (value)
		{
			case OdDgDimension.PlacementAlignment.kPaView:
				{
					strAlign = "kPaView";
				}
				break;

			case OdDgDimension.PlacementAlignment.kPaDrawing:
				{
					strAlign = "kPaDrawing";
				}
				break;

			case OdDgDimension.PlacementAlignment.kPaTrue:
				{
					strAlign = "kPaTrue";
				}
				break;

			case OdDgDimension.PlacementAlignment.kPaArbitrary:
				{
					strAlign = "kPaArbitrary";
				}
				break;
		}

		writeFieldValue(name, strAlign);
	}
	public void writeFieldValue(string name, OdDgDimTextInfo.TextLocation value)
	{
		dumpFieldName(name);

		string strVal;

		switch (value)
		{
			case OdDgDimTextInfo.TextLocation.kTextInline: strVal = "kTextInline"; break;
			case OdDgDimTextInfo.TextLocation.kTextAbove: strVal = "kTextAbove"; break;
			case OdDgDimTextInfo.TextLocation.kTextOutside: strVal = "kTextOutside"; break;
			default: strVal = "kTextTopLeft"; break;
		}

		dumpFieldValue(strVal);
	}
	public void writeFieldValue(string name, OdDgDimTool.TerminatorType iType)
	{
		switch (iType)
		{
			case OdDgDimTool.TerminatorType.kTtNone:
				{
					writeFieldValue(name, "kTtNone");
				}
				break;

			case OdDgDimTool.TerminatorType.kTtArrow:
				{
					writeFieldValue(name, "kTtArrow");
				}
				break;

			case OdDgDimTool.TerminatorType.kTtStroke:
				{
					writeFieldValue(name, "kTtStroke");
				}
				break;

			case OdDgDimTool.TerminatorType.kTtCircle:
				{
					writeFieldValue(name, "kTtCircle");
				}
				break;

			case OdDgDimTool.TerminatorType.kTtFilledCircle:
				{
					writeFieldValue(name, "kTtFilledCircle");
				}
				break;
		}

	}
	public void writeFieldValue(string name, OdDgDimTool.TextType iType)
	{
		switch (iType)
		{
			case OdDgDimTool.TextType.kStandard:
				{
					writeFieldValue(name, "kStandard");
				}
				break;

			case OdDgDimTool.TextType.kVertical:
				{
					writeFieldValue(name, "kVertical");
				}
				break;

			case OdDgDimTool.TextType.kMixed:
				{
					writeFieldValue(name, "kMixed");
				}
				break;
		}

	}
	public void writeFieldValue(string name, OdDgDimTool.CustomSymbol iSymbol)
	{
		switch (iSymbol)
		{
			case OdDgDimTool.CustomSymbol.kCsNone:
				{
					writeFieldValue(name, "kCsNone");
				}
				break;

			case OdDgDimTool.CustomSymbol.kCsDiameter:
				{
					writeFieldValue(name, "kCsDiameter");
				}
				break;

			case OdDgDimTool.CustomSymbol.kCsRadius:
				{
					writeFieldValue(name, "kCsRadius");
				}
				break;

			case OdDgDimTool.CustomSymbol.kCsSquare:
				{
					writeFieldValue(name, "kCsSquare");
				}
				break;

			case OdDgDimTool.CustomSymbol.kCsSR:
				{
					writeFieldValue(name, "kCsSR");
				}
				break;

			case OdDgDimTool.CustomSymbol.kCsSDiameter:
				{
					writeFieldValue(name, "kCsSDiameter");
				}
				break;
		}

	}
	public void writeFieldValue(string name, OdDgDimTool.Leader iLeader)
	{
		switch (iLeader)
		{
			case OdDgDimTool.Leader.kRadius:
				{
					writeFieldValue(name, "kRadius");
				}
				break;

			case OdDgDimTool.Leader.kRadiusExt1:
				{
					writeFieldValue(name, "kRadiusExt1");
				}
				break;

			case OdDgDimTool.Leader.kRadiusExt2:
				{
					writeFieldValue(name, "kRadiusExt2");
				}
				break;

			case OdDgDimTool.Leader.kDiameter:
				{
					writeFieldValue(name, "kDiameter");
				}
				break;
		}

	}
	public void writeFieldValue(string name, OdDgDimension pElement)
	{
		switch (pElement.getDimensionType())
		{
			case OdDgDimension.ToolType.kToolTypeSizeArrow:
				{
					OdDgDimSizeArrow pDimSize = pElement as OdDgDimSizeArrow;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeSizeStroke:
				{
					OdDgDimSizeStroke pDimSize = pElement as OdDgDimSizeStroke;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeLocateSingle:
				{
					OdDgDimSingleLocation pDimSize = pElement as OdDgDimSingleLocation;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeLocateStacked:
				{
					OdDgDimStackedLocation pDimSize = pElement as OdDgDimStackedLocation;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeCustomLinear:
				{
					OdDgDimCustomLinear pDimSize = pElement as OdDgDimCustomLinear;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeAngleSize:
				{
					OdDgDimAngleSize pDimSize = pElement as OdDgDimAngleSize;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeAngleLines:
				{
					OdDgDimAngleLines pDimSize = pElement as OdDgDimAngleLines;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeAngleLocation:
				{
					OdDgDimAngleLocation pDimSize = pElement as OdDgDimAngleLocation;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeArcLocation:
				{
					OdDgDimArcLocation pDimSize = pElement as OdDgDimArcLocation;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeAngleAxisX:
				{
					OdDgDimAngleAxisX pDimSize = pElement as OdDgDimAngleAxisX;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeAngleAxisY:
				{
					OdDgDimAngleAxisY pDimSize = pElement as OdDgDimAngleAxisY;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeArcSize:
				{
					OdDgDimArcSize pDimSize = pElement as OdDgDimArcSize;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeRadius:
				{
					OdDgDimRadius pDimSize = pElement as OdDgDimRadius;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeRadiusExtended:
				{
					OdDgDimRadiusExtended pDimSize = pElement as OdDgDimRadiusExtended;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeDiameter:
				{
					OdDgDimDiameter pDimSize = pElement as OdDgDimDiameter;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeDiameterExtended:
				{
					OdDgDimDiameterExtended pDimSize = pElement as OdDgDimDiameterExtended;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeDiameterPara:
				{
					OdDgDimDiameterParallel pDimSize = pElement as OdDgDimDiameterParallel;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeDiameterPerp:
				{
					OdDgDimDiameterPerpendicular pDimSize = pElement as OdDgDimDiameterPerpendicular;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeOrdinate:
				{
					OdDgDimOrdinate pDimSize = pElement as OdDgDimOrdinate;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;

			case OdDgDimension.ToolType.kToolTypeCenter:
				{
					OdDgDimCenter pDimSize = pElement as OdDgDimCenter;

					if (pDimSize != null)
						writeFieldValue(name, pDimSize);
				}
				break;
		}

	}
	public void writeFieldValue(String name, OdDgDimSizeArrow pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  JointTerminator", pElement.getJointTerminator());
		writeFieldValue("  TextType", pElement.getTextType());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(String name, OdDgDimSizeStroke pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  JointTerminator", pElement.getJointTerminator());
		writeFieldValue("  TextType", pElement.getTextType());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(String name, OdDgDimSingleLocation pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  JointTerminator", pElement.getJointTerminator());
		writeFieldValue("  TextType", pElement.getTextType());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(String name, OdDgDimStackedLocation pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  JointTerminator", pElement.getJointTerminator());
		writeFieldValue("  TextType", pElement.getTextType());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(String name, OdDgDimCustomLinear pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  JointTerminator", pElement.getJointTerminator());
		writeFieldValue("  TextType", pElement.getTextType());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}

	public void writeFieldValue(string name, OdDgDimAngleSize pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimAngleLines pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimDiameterParallel pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimDiameterPerpendicular pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}

	public void writeFieldValue(string name, OdDgDimAngleLocation pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimArcLocation pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimAngleAxisX pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimAngleAxisY pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}
	public void writeFieldValue(string name, OdDgDimRadius pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
		writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
		writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
		writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
		writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
		writeFieldValue("  Leader", pElement.getLeader());
	}
	public void writeFieldValue(string name, OdDgDimRadiusExtended pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
		writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
		writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
		writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
		writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
		writeFieldValue("  Leader", pElement.getLeader());
	}
	public void writeFieldValue(string name, OdDgDimDiameter pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
		writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
		writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
		writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
		writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
		writeFieldValue("  Leader", pElement.getLeader());
	}
	public void writeFieldValue(string name, OdDgDimDiameterExtended pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
		writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
		writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
		writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
		writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
		writeFieldValue("  Leader", pElement.getLeader());
	}

	public void writeFieldValue(string name, OdDgDimArcSize pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
		writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  ChordAlignFlag", pElement.getChordAlignFlag());
		writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
		writeFieldValue("  RightTerminator", pElement.getRightTerminator());
		writeFieldValue("  Prefix", pElement.getPrefix());
		writeFieldValue("  Suffix", pElement.getSuffix());
	}

	public void writeFieldValue(string name, OdDgDimOrdinate pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
		writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
		writeFieldValue("  DecrementInReverseDirectionFlag", pElement.getDecrementInReverseDirectionFlag());
		writeFieldValue("  FreeLocationOfTxtFlag", pElement.getFreeLocationOfTxtFlag());
		writeFieldValue("  Datum value", pElement.getDatumValue());
	}

	public void writeFieldValue(string name, OdDgDimCenter pElement)
	{
		dumpFieldName(name);
		writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
		writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
		writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
		writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
	}
	public void writeFieldValue(string name, OdDgDimOptionOffset.ChainType value)
	{
		dumpFieldName(name);

		String strValue = "";

		switch (value)
		{
			case OdDgDimOptionOffset.ChainType.kNone:
				{
					strValue = "kNone";
				}
				break;

			case OdDgDimOptionOffset.ChainType.kLine:
				{
					strValue = "kLine";
				}
				break;

			case OdDgDimOptionOffset.ChainType.kArc:
				{
					strValue = "kArc";
				}
				break;

			case OdDgDimOptionOffset.ChainType.kBSpline:
				{
					strValue = "kBSpline";
				}
				break;
		}

		dumpFieldValue(strValue);
	}
	public void writeFieldValue(string name, OdDgDimOptionOffset.LeaderAlignment value)
	{
		dumpFieldName(name);

		String strValue = "";

		switch (value)
		{
			case OdDgDimOptionOffset.LeaderAlignment.kAutoAlignment:
				{
					strValue = "kAutoAlignment";
				}
				break;

			case OdDgDimOptionOffset.LeaderAlignment.kLeftAlignment:
				{
					strValue = "kLeftAlignment";
				}
				break;

			case OdDgDimOptionOffset.LeaderAlignment.kRightAlignment:
				{
					strValue = "kRightAlignment";
				}
				break;
		}

		dumpFieldValue(strValue);
	}
	public void writeFieldValue(string name, OdDgDimTextInfo.TextAlignment value)
	{
		String strJust = "";

		switch (value)
		{
			case OdDgDimTextInfo.TextAlignment.kLeftText:
				{
					strJust = "kLeftText";
				}
				break;

			case OdDgDimTextInfo.TextAlignment.kCenterLeftText:
				{
					strJust = "kCenterLeftText";
				}
				break;

			case OdDgDimTextInfo.TextAlignment.kCenterRightText:
				{
					strJust = "kCenterRightText";
				}
				break;

			case OdDgDimTextInfo.TextAlignment.kRightText:
				{
					strJust = "kRightText";
				}
				break;

			case OdDgDimTextInfo.TextAlignment.kManualText:
				{
					strJust = "kManualText";
				}
				break;
		}

		writeFieldValue(name, strJust);
	}
	public void writeFieldValue(string name, OdDgDimTextFormat.LabelDisplayMode value)
	{
		string strAlign = "";

		switch (value)
		{
			case OdDgDimTextFormat.LabelDisplayMode.kMu:
				{
					strAlign = "Mu";
				}
				break;

			case OdDgDimTextFormat.LabelDisplayMode.kMuLabel:
				{
					strAlign = "Mu Label";
				}
				break;

			case OdDgDimTextFormat.LabelDisplayMode.kSu:
				{
					strAlign = "Su";
				}
				break;

			case OdDgDimTextFormat.LabelDisplayMode.kSuLabel:
				{
					strAlign = "Su Label";
				}
				break;

			case OdDgDimTextFormat.LabelDisplayMode.kMu_Su:
				{
					strAlign = "Mu-Su";
				}
				break;

			case OdDgDimTextFormat.LabelDisplayMode.kMuLabelSuLabel:
				{
					strAlign = "Mu Label Su Label";
				}
				break;

			case OdDgDimTextFormat.LabelDisplayMode.kMuLabel_SuLabel:
				{
					strAlign = "Mu Label-Su Label";
				}
				break;
		}

		writeFieldValue(name, strAlign);
	}
	public void writeFieldValue(string name, OdDgLevelFilterTable.OdDgFilterMemberType value)
	{
		string strValue = "";

		switch (value)
		{
			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeShort:
				{
					strValue = "kTypeShort";
				}
				break;

			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeInt:
				{
					strValue = "kTypeInt";
				}
				break;

			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeBool:
				{
					strValue = "kTypeBool";
				}
				break;

			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeChar:
				{
					strValue = "kTypeChar";
				}
				break;

			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeWChar:
				{
					strValue = "kTypeWChar";
				}
				break;

			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeDouble:
				{
					strValue = "kTypeDouble";
				}
				break;

			case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeTime:
				{
					strValue = "kTypeTime";
				}
				break;

			default:
				{
					strValue = "kTypeNull";
				}
				break;
		}

		writeFieldValue(name, strValue);
	}
	public void writeFieldValue(string name, OdDgGradientFillLinkage.OdDgGradientType value)
	{
		string strValue;

		switch (value)
		{
			case OdDgGradientFillLinkage.OdDgGradientType.kCurved:
				{
					strValue = "kCurved";
				}
				break;

			case OdDgGradientFillLinkage.OdDgGradientType.kCylindrical:
				{
					strValue = "kCylindrical";
				}
				break;

			case OdDgGradientFillLinkage.OdDgGradientType.kSpherical:
				{
					strValue = "kSpherical";
				}
				break;

			case OdDgGradientFillLinkage.OdDgGradientType.kHemispherical:
				{
					strValue = "kHemispherical";
				}
				break;

			default:
				{
					strValue = "kLinear";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgLineStyleModificationLinkage.OdDgLsModWidthMode value)
	{
		string strValue;

		switch (value)
		{
			case OdDgLineStyleModificationLinkage.OdDgLsModWidthMode.kLsModConstantWidth:
				{
					strValue = "kConstantWidth";
				}
				break;

			case OdDgLineStyleModificationLinkage.OdDgLsModWidthMode.kLsModTaperedWidth:
				{
					strValue = "kTaperedWidth";
				}
				break;

			default:
				{
					strValue = "kNoWidth";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgLineStyleModificationLinkage.OdDgLsModShiftMode value)
	{
		string strValue;

		switch (value)
		{
			case OdDgLineStyleModificationLinkage.OdDgLsModShiftMode.kLsModCentered:
				{
					strValue = "kCentered";
				}
				break;

			case OdDgLineStyleModificationLinkage.OdDgLsModShiftMode.kLsModDistance:
				{
					strValue = "kDistance";
				}
				break;

			case OdDgLineStyleModificationLinkage.OdDgLsModShiftMode.kLsModFraction:
				{
					strValue = "kFraction";
				}
				break;

			default:
				{
					strValue = "kNoShift";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgLineStyleModificationLinkage.OdDgLsModCornerMode value)
	{
		string strValue;

		switch (value)
		{
			case OdDgLineStyleModificationLinkage.OdDgLsModCornerMode.kLsModBreakAtCorners:
				{
					strValue = "kBreakAtCorners";
				}
				break;

			case OdDgLineStyleModificationLinkage.OdDgLsModCornerMode.kLsModRunThroughCorners:
				{
					strValue = "kRunThroughCorners";
				}
				break;

			default:
				{
					strValue = "kFromLineStyle";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgDependencyLinkage.RootDataType value)
	{
		string strValue;

		switch (value)
		{
			case OdDgDependencyLinkage.RootDataType.kElementId:
				{
					strValue = "kElementId";
				}
				break;

			case OdDgDependencyLinkage.RootDataType.kElementId_V:
				{
					strValue = "kElementId V";
				}
				break;

			case OdDgDependencyLinkage.RootDataType.kAssocPoint:
				{
					strValue = "kAssocPoint";
				}
				break;

			case OdDgDependencyLinkage.RootDataType.kAssocPoint_I:
				{
					strValue = "kAssocPoint I";
				}
				break;

			case OdDgDependencyLinkage.RootDataType.kFarElementId:
				{
					strValue = "kFarElementId";
				}
				break;

			case OdDgDependencyLinkage.RootDataType.kFarElementId_V:
				{
					strValue = "kFarElementId V";
				}
				break;

			case OdDgDependencyLinkage.RootDataType.kPath_V:
				{
					strValue = "kPath V";
				}
				break;

			default:
				{
					strValue = "kUnknownType";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgAssocPointRoot.OdDgAssocPointRootType value)
	{
		string strValue;

		switch (value)
		{
			case OdDgAssocPointRoot.OdDgAssocPointRootType.kLinearAssociation:
				{
					strValue = "kLinearAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kIntersectAssociation:
				{
					strValue = "kIntersectAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kArcAssociation:
				{
					strValue = "kArcAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kMultilineAssociation:
				{
					strValue = "kMultilineAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kBSplineCurveAssociation:
				{
					strValue = "kBSplineCurveAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kProjectionAssociation:
				{
					strValue = "kProjectionAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kOriginAssociation:
				{
					strValue = "kOriginAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kIntersect2Association:
				{
					strValue = "kIntersect2Association";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kMeshVertexAssociation:
				{
					strValue = "kMeshVertexAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kMeshEdgeAssociation:
				{
					strValue = "kMeshEdgeAssociation";
				}
				break;

			case OdDgAssocPointRoot.OdDgAssocPointRootType.kBSplineSurfaceAssociation:
				{
					strValue = "kBSplineSurfaceAssociation";
				}
				break;

			default:
				{
					strValue = "kUnknownAssociation";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgArcAssociation.OdDgArcAssociationKeyPoint value)
	{
		string strValue;

		switch (value)
		{
			case OdDgArcAssociation.OdDgArcAssociationKeyPoint.kCenterPoint:
				{
					strValue = "kCenterPoint";
				}
				break;

			case OdDgArcAssociation.OdDgArcAssociationKeyPoint.kStartPoint:
				{
					strValue = "kStartPoint";
				}
				break;

			case OdDgArcAssociation.OdDgArcAssociationKeyPoint.kEndPoint:
				{
					strValue = "kEndPoint";
				}
				break;

			default:
				{
					strValue = "kAnglePoint";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgOriginAssociation.OdDgOriginAssocOption value)
	{
		string strValue;

		switch (value)
		{
			case OdDgOriginAssociation.OdDgOriginAssocOption.kInsertionPoint:
				{
					strValue = "kInsertionPoint";
				}
				break;

			default:
				{
					strValue = "kUpperLeftPoint";
				}
				break;
		}

		writeFieldValue(name, strValue);

	}
	public void writeFieldValue(string name, OdDgInternalMaterialLinkage.OdDgInternalMaterialType value)
	{
		string strValue;

		switch (value)
		{
			case OdDgInternalMaterialLinkage.OdDgInternalMaterialType.kLevelOverride:
				{
					strValue = "kLevelOverride";
				}
				break;
			case OdDgInternalMaterialLinkage.OdDgInternalMaterialType.kByLevelAssigned:
				{
					strValue = "kByLevelAssigned";
				}
				break;
			default:
				{
					strValue = "kElementAssigned";
				}
				break;
		}
		writeFieldValue(name, strValue);
	}
};

class OdDgRxObjectDumperPE
{
	public virtual void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{

	}
	static Dictionary<string, OdDgRxObjectDumperPE> dic = new Dictionary<string, OdDgRxObjectDumperPE>();
	public static OdDgRxObjectDumperPE getDumper(OdRxClass c)
	{
		for (OdRxClass pc = c; pc != null; pc = pc.myParent())
		{
			string name = pc.name();
			OdDgRxObjectDumperPE res;
			if (dic.TryGetValue(name, out res))
				return res;
		}
		return null;
	}
	public static void registerDumper(string name, OdDgRxObjectDumperPE d)
	{
		dic[name] = d;
	}
}
class OdDgElementDumperPE : OdDgRxObjectDumperPE
{
	public virtual OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		return null;
	}
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		dumpData(pObj, pDumper);
		dumpLinkages(pObj, pDumper);
		dumpXAttributes(pObj, pDumper);
		dumpReactors(pObj, pDumper);
	}

	public virtual string getName(OdRxObject pObj)
	{
		return String.Format("<{0}>", pObj.isA().name());
	}

	public virtual bool hasSubElements(OdDgElement pElm)
	{
		OdDgElementIterator pIt = createIterator(pElm, true, true);
		return pIt != null && !pIt.done();
	}

	protected virtual void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		OdDgElement pElm = (OdDgElement)pObj;
		pDumper.writeFieldValue("ElementId", pElm.elementId().getHandle().ascii());
		pDumper.writeFieldValue("OwnerId", pElm.ownerId().getHandle().ascii());
	}

	protected virtual void dumpLinkages(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		OdDgElement pElm = (OdDgElement)pObj;

		OdRxObjectPtrArray linkages = new OdRxObjectPtrArray();
		pElm.getLinkages(linkages);
		pDumper.writeFieldValue("Linkages Count", (UInt32)linkages.Count);
		if (linkages.Count > 0)
		{
			for (int i = 0; i < linkages.Count; ++i)
			{
				OdDgAttributeLinkage pLinkage = (OdDgAttributeLinkage)linkages[i];
				OdDgAttributeLinkageHeader header = pLinkage.getLinkageHeader(false);
				OdBinaryData data = new OdBinaryData();
				pLinkage.getData(data);

				OdDgAttributeLinkage.PrimaryIds primaryId = (OdDgAttributeLinkage.PrimaryIds)pLinkage.getPrimaryId();
				pDumper.writeFieldValueHex("Linkage", (ushort)primaryId);
				pDumper.writeFieldValue("  Class name", pLinkage.isA().name());
				pDumper.writeFieldValue("  Raw Data Size", data.Count);
				pDumper.writeFieldValue("  Modified Linkage Flag", header.getModifiedFlag());
				pDumper.writeFieldValue("  Remote Linkage Flag", header.getRemoteFlag());
				pDumper.writeFieldValue("  User Linkage Flag", header.getUserDataFlag());
				pDumper.writeFieldValue("  Info Linkage Flag", header.getInfoLinkageFlag());
				switch (primaryId)
				{
					case OdDgAttributeLinkage.PrimaryIds.kLStyleMod:
						{
							OdDgLineStyleModificationLinkage pLStyleModLinkage = (OdDgLineStyleModificationLinkage)pLinkage;

							if (pLStyleModLinkage.getUseLineStyleScaleFlag())
							{
								pDumper.writeFieldValue("  Line Style Scale", pLStyleModLinkage.getLineStyleScale());
							}

							if (pLStyleModLinkage.getUseLineStyleDashScaleFlag())
							{
								pDumper.writeFieldValue("  Line Style Dash Scale", pLStyleModLinkage.getLineStyleDashScale());
							}

							if (pLStyleModLinkage.getUseLineStyleGapScaleFlag())
							{
								pDumper.writeFieldValue("  Line Style Gap Scale", pLStyleModLinkage.getLineStyleGapScale());
							}

							pDumper.writeFieldValue("  Width Mode", pLStyleModLinkage.getWidthMode());

							if (pLStyleModLinkage.getWidthMode() == OdDgLineStyleModificationLinkage.OdDgLsModWidthMode.kLsModConstantWidth)
							{
								pDumper.writeFieldValue("  Width", pLStyleModLinkage.getLineStyleWidth());
								pDumper.writeFieldValue("  True Width Flag", pLStyleModLinkage.getUseLineStyleTrueWidthFlag());
							}
							else if (pLStyleModLinkage.getWidthMode() == OdDgLineStyleModificationLinkage.OdDgLsModWidthMode.kLsModTaperedWidth)
							{
								pDumper.writeFieldValue("  Start Width", pLStyleModLinkage.getLineStyleWidth());
								pDumper.writeFieldValue("  End Width", pLStyleModLinkage.getLineStyleEndWidth());
								pDumper.writeFieldValue("  True Width Flag", pLStyleModLinkage.getUseLineStyleTrueWidthFlag());
							}

							pDumper.writeFieldValue("  Shift Mode", pLStyleModLinkage.getShiftMode());

							if (pLStyleModLinkage.getShiftMode() == OdDgLineStyleModificationLinkage.OdDgLsModShiftMode.kLsModDistance)
							{
								pDumper.writeFieldValue("  Shift Distance", pLStyleModLinkage.getLineStyleShift());
							}
							else if (pLStyleModLinkage.getShiftMode() == OdDgLineStyleModificationLinkage.OdDgLsModShiftMode.kLsModFraction)
							{
								pDumper.writeFieldValue("  Fraction Phase", pLStyleModLinkage.getLineStyleFractionPhase());
							}

							pDumper.writeFieldValue("  Corner Mode", pLStyleModLinkage.getCornerMode());

							if (pLStyleModLinkage.getUseLineStyleMultilineDataFlag())
							{
								pDumper.writeFieldValue("  Multiline Data", pLStyleModLinkage.getMultilineDataType());

								if (pLStyleModLinkage.getMultilineDataType() == OdDgLineStyleModificationLinkage.OdDgLsMultilineDataType.kLsMultilineTypeLine)
								{
									OdUInt32Array indexArr = new OdUInt32Array();
									pLStyleModLinkage.getMultilineProfileIndexes(indexArr);

									if (indexArr.Count != 0)
									{
										pDumper.writeFieldValue("  Multiline Profile Index", indexArr[0]);
									}
								}
							}
						}
						break;

					case OdDgAttributeLinkage.PrimaryIds.kFillStyle:
						{
							OdDgFillColorLinkage pFillColor = pLinkage as OdDgFillColorLinkage;

							if (pFillColor != null)
							{
								pDumper.writeFieldValue("  Fill color", pFillColor.getColorIndex());
								break;
							}

							OdDgTransparencyLinkage pTransparency = pLinkage as OdDgTransparencyLinkage;

							if (pTransparency != null)
							{
								pDumper.writeFieldValue("  Transparency", pTransparency.getTransparency());
								break;
							}

							OdDgInternalMaterialLinkage pMaterial = pLinkage as OdDgInternalMaterialLinkage;

							if (pMaterial != null)
							{
								pDumper.writeFieldValue("  Linkage Type", pMaterial.getMaterialType());
								pDumper.writeFieldValue("  Material Id", pMaterial.getMaterialTableRecordId());
								break;
							}

							OdDgGradientFillLinkage pGradient = pLinkage as OdDgGradientFillLinkage;

							if (pLinkage != null)
							{
								pDumper.writeFieldValue("  Gradient Type", pGradient.getGradientType());
								pDumper.writeFieldValue("  Gradient Angle", pGradient.getAngle());
								pDumper.writeFieldValue("  White Intensity", pGradient.getWhiteIntensity());
								pDumper.writeFieldValue("  Invert Flag", pGradient.getInvertFlag());
								pDumper.writeFieldValue("  Key Count", pGradient.getKeyCount());

								for (UInt16 k = 0; k < pGradient.getKeyCount(); k++)
								{
									OdDgGradientFillLinkage.OdDgGradientKey key = pGradient.getKey(k);
									pDumper.writeFieldValue("    Key Number", k);
									pDumper.writeFieldValue("    Key Position", key.dKeyPosition);
									pDumper.writeFieldValueHex("    Key Color", key.clrKeyColor);
								}

								break;
							}

						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kBREP:
						{
							OdDgBrepLinkage pBrepLinkage = (OdDgBrepLinkage)pLinkage;
							pDumper.writeFieldValue("  Brep Flag1", pBrepLinkage.getFlag1());
							pDumper.writeFieldValue("  Brep Flag2", pBrepLinkage.getFlag2());
							pDumper.writeFieldValue("  Brep Scale", pBrepLinkage.getScale());
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kDependency:
						{
							OdDgDependencyLinkage pDepLinkage = (OdDgDependencyLinkage)pLinkage;
							pDumper.writeFieldValue("  Dependency AppId", pDepLinkage.getAppId());
							pDumper.writeFieldValue("  Dependency AppValue", pDepLinkage.getAppValue());
							pDumper.writeFieldValue("  Dependency RootDataType", pDepLinkage.getRootDataType());

							switch (pDepLinkage.getRootDataType())
							{
								case OdDgDependencyLinkage.RootDataType.kElementId:
									{
										OdDgDepLinkageElementId pDepLinkageElmId = (OdDgDepLinkageElementId)pDepLinkage;
										uint idCount = pDepLinkageElmId.getCount();
										for (UInt32 j = 0; j < idCount; ++j)
										{
											pDumper.writeFieldValue("  Root Number", j);
											pDumper.writeFieldValueHex("    Dependency ElementId", pDepLinkageElmId.getAt(j));
										}
									}
									break;

								case OdDgDependencyLinkage.RootDataType.kElementId_V:
									{
										OdDgDepLinkageElementIdV pDepLinkageElmId = (OdDgDepLinkageElementIdV)pDepLinkage;
										var idCount = pDepLinkageElmId.getCount();

										for (UInt32 j = 0; j < idCount; ++j)
										{
											OdDgElementIdVData rootData = pDepLinkageElmId.getAt(j);

											pDumper.writeFieldValue("  Root Number", j);
											pDumper.writeFieldValueHex("    Dependency ElementId", rootData.m_uId);
											pDumper.writeFieldValue("    Value", rootData.m_dValue);
										}
									}
									break;

								case OdDgDependencyLinkage.RootDataType.kAssocPoint:
									{
										OdDgDepLinkageAssocPoint pDepLinkageAssocPt = (OdDgDepLinkageAssocPoint)pDepLinkage;
										var idCount = pDepLinkageAssocPt.getCount();

										for (UInt32 j = 0; j < idCount; ++j)
										{
											OdDgAssocPointRoot pRootData = pDepLinkageAssocPt.getAt(j);

											pDumper.writeFieldValue("  Root Number", j);

											OdDgRxObjectDumperPE pAssocRootDumper = OdDgRxObjectDumperPE.getDumper(pRootData.isA());

											if (pAssocRootDumper != null)
											{
												pAssocRootDumper.dump(pRootData, pDumper);
											}
										}
									}
									break;

								case OdDgDependencyLinkage.RootDataType.kAssocPoint_I:
									{
										OdDgDepLinkageAssocPointI pDepLinkageAssocPt = (OdDgDepLinkageAssocPointI)pDepLinkage;
										var idCount = pDepLinkageAssocPt.getCount();

										for (UInt32 j = 0; j < idCount; ++j)
										{
											/*tmp#18875*/
											OdDgAssocPointIData rootData = pDepLinkageAssocPt.getAt(j);

											pDumper.writeFieldValue("  Root Number", j);

											//tmp#18875 
											OdDgRxObjectDumperPE pAssocRootDumper = OdDgRxObjectDumperPE.getDumper(rootData.getPointData().isA());

											//tmp#18875 
											if (pAssocRootDumper != null)
											{
												pAssocRootDumper.dump(rootData.getPointData(), pDumper);
											}

											//tmp#18875 
											pDumper.writeFieldValue("    Int 1", rootData.m_iParam1);
											//tmp#18875 
											pDumper.writeFieldValue("    Int 2", rootData.m_iParam2);
										}
									}
									break;

								case OdDgDependencyLinkage.RootDataType.kFarElementId:
									{
										OdDgDepLinkageFarElementId pDepLinkageElmId = (OdDgDepLinkageFarElementId)pDepLinkage;
										var idCount = pDepLinkageElmId.getCount();

										for (UInt32 j = 0; j < idCount; ++j)
										{
											OdDgFarElementIdData rootData = pDepLinkageElmId.getAt(j);

											pDumper.writeFieldValue("  Root Number", j);
											pDumper.writeFieldValueHex("    Dependency ElementId", rootData.m_elementId);
											pDumper.writeFieldValueHex("    XRef ElementId", rootData.m_referenceAttachId);
										}
									}
									break;

								case OdDgDependencyLinkage.RootDataType.kFarElementId_V:
									{
										OdDgDepLinkageFarElementIdV pDepLinkageElmId = (OdDgDepLinkageFarElementIdV)pDepLinkage;
										var idCount = pDepLinkageElmId.getCount();

										for (UInt32 j = 0; j < idCount; ++j)
										{
											OdDgFarElementIdVData rootData = pDepLinkageElmId.getAt(j);

											pDumper.writeFieldValue("  Root Number", i);
											pDumper.writeFieldValueHex("    Dependency ElementId", rootData.m_elementId);
											pDumper.writeFieldValueHex("    XRef ElementId", rootData.m_referenceAttachId);
											pDumper.writeFieldValue("    Double Value", rootData.m_dParam);
										}
									}
									break;

								case OdDgDependencyLinkage.RootDataType.kPath_V:
									{
										OdDgDepLinkagePath pDepLinkagePath = (OdDgDepLinkagePath)pDepLinkage;
										var idCount = pDepLinkagePath.getCount();

										for (UInt32 j = 0; j < idCount; ++j)
										{
											OdDgDependencyPathData rootData = pDepLinkagePath.getAt(j);

											pDumper.writeFieldValue("  Root Number", j);
											pDumper.writeFieldValue("    Double Value", rootData.m_dParam);

											for (int l = 0; l < rootData.m_referenceAttachPath.Count; l++)
											{
												String strFieldName = String.Format("    Path item {0}", l);
												pDumper.writeFieldValueHex(strFieldName, rootData.m_referenceAttachPath[l]);
											}
										}
									}
									break;
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kThickness:
						{
							OdDgThicknessLinkage pThicknessLinkage = (OdDgThicknessLinkage)pLinkage;

							double dThickness = pThicknessLinkage.getThickness();

							OdDgDatabase pDb = pElm.database();
							OdDgElementId idModel = null;

							if (pDb != null)
								idModel = pDb.getActiveModelId();

							if (idModel != null)
							{
								OdDgModel pModel = OdDgModel.cast(idModel.openObject());

								if (pModel != null)
								{
									dThickness = pModel.convertUORsToWorkingUnits(dThickness);
								}
							}
							else
							{
								dThickness /= 10000000000; // Storage units default factor
							}

							pDumper.writeFieldValue("  Thickness", dThickness);
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kFilterMember:
						{
							OdDgFilterMemberLinkage pFilterLinkage = (OdDgFilterMemberLinkage)pLinkage;
							pDumper.writeFieldValue("  Member Id", pFilterLinkage.getMemberId());
							pDumper.writeFieldValue("  Member Type", pFilterLinkage.getMemberType());
							pDumper.writeFieldValue("  Name String", pFilterLinkage.getNameString());
							pDumper.writeFieldValue("  Expression String", pFilterLinkage.getExpressionString());
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kString:
						{
							OdDgStringLinkage pStringLinkage = (OdDgStringLinkage)pLinkage;
							pDumper.writeFieldValue("  String Id", pStringLinkage.getStringId());
							pDumper.writeFieldValue("  String Data", pStringLinkage.getString());
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kByteArray:
						{
							OdDgByteArrayLinkage pByteArrayLinkage = (OdDgByteArrayLinkage)pLinkage;
							pDumper.writeFieldValue("  ByteArray Id", pByteArrayLinkage.getArrayId());
							OdBinaryData data1 = new OdBinaryData();
							pByteArrayLinkage.getArrayData(data1);
							pDumper.writeFieldValue("  ByteArray Data Size", data1.Count);
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kDoubleArray:
						{
							OdDgDoubleArrayLinkage pDoubleArrayLinkage = (OdDgDoubleArrayLinkage)pLinkage;
							pDumper.writeFieldValue("  DoubleArray Id", pDoubleArrayLinkage.getArrayId());

							for (UInt32 n = 0; n < pDoubleArrayLinkage.getItemCount(); n++)
							{
								String strFieldName;
								strFieldName = String.Format("  Double [{0}]", n);
								pDumper.writeFieldValue(strFieldName, pDoubleArrayLinkage.getItem(n));
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kHatch:
						{
							OdDgPatternLinkage pPatternLinkage = (OdDgPatternLinkage)pLinkage;
							pDumper.writeFieldValue("  Pattern Type", (UInt32)pPatternLinkage.getType());
							if (pPatternLinkage.getUseOffsetFlag())
							{
								OdGePoint3d offset = new OdGePoint3d();
								pPatternLinkage.getOffset(offset);
								pDumper.writeFieldValue("  offset", offset);
							}

							if (pPatternLinkage.getType() == OdDgPatternLinkage.PatternType.kDWGPattern)
							{
								OdDgDWGPatternLinkage pDWGPattern = (OdDgDWGPatternLinkage)pPatternLinkage;

								pDumper.dumpFieldName("  DWG hatch:");

								DWGHatch hatch = pDWGPattern.getHatch();

								double dScaleFactor = 1.0;

								OdDgDatabase pDb = pElm.database();
								OdDgElementId idModel = null;

								if (pDb != null)
									idModel = pDb.getActiveModelId();

								if (idModel != null)
								{
									OdDgModel pModel = OdDgModel.cast(idModel.openObject());

									if (pModel != null)
									{
										dScaleFactor = pModel.convertUORsToWorkingUnits(1.0);
									}
								}

								for (int k = 0; k < hatch.Count; k++)
								{
									OdDgDWGPatternLinkage.DWGHatchLine hatchLine = hatch[k];

									OdGePoint2d ptOffset = hatchLine.m_offset;
									OdGePoint2d ptThrough = hatchLine.m_throughPoint;

									pDumper.writeFieldValue("    Hatch line Number", k);
									pDumper.writeFieldValue("    Hatch line Angle", hatchLine.m_angle);
									pDumper.writeFieldValue("    Hatch line Offset", ptOffset * dScaleFactor);
									pDumper.writeFieldValue("    Hatch line Through pt", ptThrough * dScaleFactor);

									if (hatchLine.m_dashes.Count != 0)
									{
										pDumper.dumpFieldName("    Hatch line dashes:");
									}

									for (int n = 0; n < hatchLine.m_dashes.Count; n++)
									{
										String strFieldName = String.Format("      Dash {0}", n);
										pDumper.writeFieldValue(strFieldName, hatchLine.m_dashes[n] * dScaleFactor);
									}
								}
							}
							else if (pPatternLinkage.getType() == OdDgPatternLinkage.PatternType.kSymbolPattern)
							{
								OdDgSymbolPatternLinkage pSymPattern = (OdDgSymbolPatternLinkage)pPatternLinkage;

								pDumper.writeFieldValue("  Angle1", pSymPattern.getAngle1());
								pDumper.writeFieldValue("  Angle2", pSymPattern.getAngle2());
								pDumper.writeFieldValue("  Space1", pSymPattern.getSpace1());
								pDumper.writeFieldValue("  Space2", pSymPattern.getSpace2());
								pDumper.writeFieldValue("  Scale", pSymPattern.getScale());
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kDimension:
						{
							OdDgDimensionLinkage pDimLinkage = (OdDgDimensionLinkage)pLinkage;

							if (pDimLinkage != null && pDimLinkage.getType() == OdDgDimensionLinkage.DimensionSubType.kDimensionInfo)
							{
								OdDgDimensionInfoLinkage pDimInfoLinkage = (OdDgDimensionInfoLinkage)pLinkage;

								if (pDimInfoLinkage.getUseAnnotationScale())
									pDumper.writeFieldValue("  Annotation Scale", pDimInfoLinkage.getAnnotationScale());

								if (pDimInfoLinkage.getUseDatumValue())
								{
									double dDatumValue = pDimInfoLinkage.getDatumValue();

									OdDgDatabase pDb = pElm.database();
									OdDgElementId idModel = null;

									if (pDb != null)
										idModel = pDb.getActiveModelId();

									if (idModel != null)
									{
										OdDgModel pModel = OdDgModel.cast(idModel.openObject());

										if (pModel != null)
										{
											dDatumValue = pModel.convertUORsToWorkingUnits(dDatumValue);
										}
									}
									else
									{
										dDatumValue /= 10000000000; // Storage units default factor
									}

									pDumper.writeFieldValue("  Datum Value", dDatumValue);
								}

								if (pDimInfoLinkage.getUseRetainFractionalAccuracy())
								{
									pDumper.writeFieldValue("  Detriment in reverse direction", pDimInfoLinkage.getUseDecrimentInReverceDirection());
									pDumper.writeFieldValue("  Primary retain fractional accuracy", pDimInfoLinkage.getPrimaryRetainFractionalAccuracy());
									pDumper.writeFieldValue("  Secondary retain fractional accuracy", pDimInfoLinkage.getSecondaryRetainFractionalAccuracy());
									pDumper.writeFieldValue("  Primary alt format retain fractional accuracy", pDimInfoLinkage.getPrimaryAltFormatRetainFractionalAccuracy());
									pDumper.writeFieldValue("  Secondary alt format retain fractional accuracy", pDimInfoLinkage.getSecondaryAltFormatRetainFractionalAccuracy());
									pDumper.writeFieldValue("  Primary tolerance retain fractional accuracy", pDimInfoLinkage.getPrimaryTolerRetainFractionalAccuracy());
									pDumper.writeFieldValue("  Secondary tolerance retain fractional accuracy", pDimInfoLinkage.getSecondaryTolerRetainFractionalAccuracy());
									pDumper.writeFieldValue("  Label line mode", pDimInfoLinkage.getLabelLineDimensionMode());
								}

								if (pDimInfoLinkage.getUseFitOptionsFlag())
								{
									pDumper.writeFieldValue("  Suppress unfit terminators", pDimInfoLinkage.getUseSuppressUnfitTerm());
									pDumper.writeFieldValue("  Use inline leader length", pDimInfoLinkage.getUseInlineLeaderLength());
									pDumper.writeFieldValue("  Text above optimal fit", pDimInfoLinkage.getUseTextAboveOptimalFit());
									pDumper.writeFieldValue("  Narrow font optimal fit", pDimInfoLinkage.getUseNarrowFontOptimalFit());
									pDumper.writeFieldValue("  Use Min Leader Terminator Length", pDimInfoLinkage.getUseMinLeaderTermLength());
									pDumper.writeFieldValue("  Use auto mode for dimension leader", pDimInfoLinkage.getUseAutoLeaderMode());
									pDumper.writeFieldValue("  Fit Options ", pDimInfoLinkage.getFitOptions());
								}

								if (pDimInfoLinkage.getUseTextLocation())
								{
									pDumper.writeFieldValue("  Free location of text", pDimInfoLinkage.getUseFreeLocationText());
									pDumper.writeFieldValue("  Note spline fit", pDimInfoLinkage.getUseNoteSplineFit());
									pDumper.writeFieldValue("  Text location ", pDimInfoLinkage.getTextLocation());
								}

								if (pDimInfoLinkage.getUseInlineLeaderLengthValue())
								{
									pDumper.writeFieldValue("   leader length value", pDimInfoLinkage.getInlineLeaderLength());
								}

								if (pDimInfoLinkage.getUseInlineTextLift())
									pDumper.writeFieldValue("   text lift", pDimInfoLinkage.getInlineTextLift());

								if (pDimInfoLinkage.getUseNoteFrameScale())
									pDumper.writeFieldValue("  Note frame scale", pDimInfoLinkage.getUseNoteFrameScale());

								if (pDimInfoLinkage.getUseNoteLeaderLength())
									pDumper.writeFieldValue("  Note leader length", pDimInfoLinkage.getNoteLeaderLength());

								if (pDimInfoLinkage.getUseNoteLeftMargin())
									pDumper.writeFieldValue("  Note left margin", pDimInfoLinkage.getUseNoteLeftMargin());

								if (pDimInfoLinkage.getUseNoteLowerMargin())
									pDumper.writeFieldValue("  Note lower margin", pDimInfoLinkage.getUseNoteLowerMargin());

								if (pDimInfoLinkage.getUsePrimaryToleranceAccuracy())
									pDumper.writeFieldValue("  Primary tolerance accuracy", pDimInfoLinkage.getPrimaryToleranceAccuracy());

								if (pDimInfoLinkage.getUseSecondaryToleranceAccuracy())
									pDumper.writeFieldValue("  Secondary tolerance accuracy", pDimInfoLinkage.getSecondaryToleranceAccuracy());

								if (pDimInfoLinkage.getUseStackedFractionScale())
									pDumper.writeFieldValue("  Stacked fraction scale", pDimInfoLinkage.getStackedFractionScale());
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kReferenceAttachmentPath:
						{
							OdDgReferenceAttachPathLinkage pPathLink = (OdDgReferenceAttachPathLinkage)linkages[i];

							if (pPathLink != null)
							{
								for (UInt32 l = 0; l < pPathLink.getPathLength(); l++)
								{
									String strFieldName;
									strFieldName = String.Format("  Path item {0}", l);
									pDumper.writeFieldValueHex(strFieldName, pPathLink.getPathItem(l));
								}
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kOLE:
						{
							OdDgOleLinkage pOleLinkage = (OdDgOleLinkage)linkages[i];

							pDumper.writeFieldValue("  Draw Aspect", pOleLinkage.getDrawAspect());
							pDumper.writeFieldValue("  Ole Type", pOleLinkage.getOleType());
							pDumper.writeFieldValue("  Size X ( in cm )", pOleLinkage.getXSize() / 1000.0);
							pDumper.writeFieldValue("  Size Y ( in cm )", pOleLinkage.getYSize() / 1000.0);
							pDumper.writeFieldValue("  Aspect Ratio Flag", pOleLinkage.getAspectRatioFlag());
							pDumper.writeFieldValue("  Transparent Background Flag", pOleLinkage.getTransparentBackgroundFlag());
							pDumper.writeFieldValue("  Rotate With View Flag", pOleLinkage.getRotateWithViewFlag());
							pDumper.writeFieldValue("  View Projection Mode Flag", pOleLinkage.getViewProjectionModeFlag());
							pDumper.writeFieldValue("  Can Be Picture Flag", pOleLinkage.getCanBePictureFlag());
							pDumper.writeFieldValue("  Can Be Linked and Embedded Flag", pOleLinkage.getCanBeLinkedAndEmbeddedFlag());
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kSeedPoints:
						{
							OdDgSeedPointsLinkage pSeedPoints = (OdDgSeedPointsLinkage)linkages[i];

							for (UInt32 iPt = 0; iPt < pSeedPoints.getPointsCount(); iPt++)
							{
								String strName;
								strName = String.Format("Point {0}", iPt);
								pDumper.writeFieldValue(strName, pSeedPoints.getPointAt(iPt));
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kBitMaskLinkage:
						{

							OdDgTextWhiteSpaceLinkage pTextBitMaskLinkage = (OdDgTextWhiteSpaceLinkage)pLinkage;

							if (pTextBitMaskLinkage != null)
							{
								pDumper.writeFieldValue("  Symbol Count", pTextBitMaskLinkage.getSymbolCount());

								for (UInt32 iSymbol = 0; iSymbol < pTextBitMaskLinkage.getSymbolCount(); iSymbol++)
								{
									String strSymbolName;
									strSymbolName = String.Format("    Symbol {0}", iSymbol);
									pDumper.writeFieldValue(strSymbolName, pTextBitMaskLinkage.getSymbol(iSymbol));
								}

								pDumper.writeFieldValue("  Symbol String", pTextBitMaskLinkage.createString());
							}

							OdDgBitMaskLinkage pBitMaskLinkage = (OdDgBitMaskLinkage)pLinkage;

							if (pBitMaskLinkage != null)
							{
								pDumper.writeFieldValue("  BitMask Id", pBitMaskLinkage.getBitMaskId());
								pDumper.writeFieldValue("  Bit Count", pBitMaskLinkage.getBitCount());

								OdBinaryData binData;

								binData = pBitMaskLinkage.getBitMaskData();

								String strData = "";

								for (Int32 iWord = 0; iWord < binData.Count / 2; iWord++)
								{
									String strWord;

									strWord = String.Format("0x{0:x2},", binData[iWord]);

									strData += strWord;
								}

								if (strData.Length > 0)
								{
									strData = strData.Substring(0, strData.Length - 1);
								}

								pDumper.writeFieldValue("  Binary Data", strData);
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kTextIndentation:
						{
							OdDgTextIndentationLinkage pTextLinkage = (OdDgTextIndentationLinkage)linkages[i];

							pDumper.writeFieldValue("  First Line Indentation", pTextLinkage.getFirstLineIndentation());
							pDumper.writeFieldValue("  Paragraph Indentation", pTextLinkage.getParagraphIndentation());
							pDumper.writeFieldValue("  Number of Tabs", pTextLinkage.getTabCount());

							for (UInt32 iTab = 0; iTab < pTextLinkage.getTabCount(); iTab++)
							{
								pDumper.writeFieldValue("    Tab Value", pTextLinkage.getTabValue(iTab));
							}
						}
						break;
					case OdDgAttributeLinkage.PrimaryIds.kDimTextStyle:
						{
							OdDgDimensionTextStyleDataLinkage pTextStyleLinkage = (OdDgDimensionTextStyleDataLinkage)linkages[i];

							OdDgTextStyleTableRecord pTextStyle = pTextStyleLinkage.createTextStyle(pElm.database());

							pDumper.writeFieldValue("---Text Style---", "---------------------------");

							OdDgTextStyleTableRecordDumperPE pTextStyleDumperPE = OdDgRxObjectDumperPE.getDumper(pTextStyle.isA()) as OdDgTextStyleTableRecordDumperPE;

							if (pTextStyleDumperPE != null)
							{
								pTextStyleDumperPE.dump(pTextStyle, pDumper);
							}

						}
						break;
				}

				try
				{
					OdDgProxyLinkage pProxyLinkage = (OdDgProxyLinkage)pLinkage;

					if (pProxyLinkage != null)
					{
						OdBinaryData binData = new OdBinaryData();

						pProxyLinkage.getData(binData);

						dumpBinaryData(binData, pDumper);
					}
				}
				catch { }
			}
		}
	}

	protected virtual void dumpBinaryData(OdBinaryData binData, OdExDgnDumper pDumper)
	{

		String strDataLine;
		String strData = "";
		UInt32 nData = 0;
		UInt32 uDataLine = 0;

		//UInt16 pWordData = (UInt16)(binData.asArrayPtr());

		for (Int32 l = 0; l < binData.Count; l++)
		{
			String strByte;
			strByte = String.Format("0x{0:X2}, ", binData[l]);
			strData += strByte;
			nData++;

			if (nData == 8)
			{
				strData = strData.Substring(0, strData.Length - 2);
				strDataLine = String.Format("  Line {0}", uDataLine);
				uDataLine++;

				pDumper.writeFieldValue(strDataLine, strData);
				strData = "";
				nData = 0;
			}
		}

		if (strData != "")
		{
			strData = strData.Substring(0, strData.Length - 2);
			strDataLine = String.Format("  Line {0}", uDataLine);

			pDumper.writeFieldValue(strDataLine, strData);
		}
	}

	protected virtual void dumpXAttributes(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		OdDgElement pElm = (OdDgElement)pObj;

		OdRxObjectPtrArray xattributes = new OdRxObjectPtrArray();
		pElm.getXAttributes(xattributes);
		pDumper.writeFieldValue("XAttributes Count", xattributes.Count);
		if (xattributes.Count > 0)
		{
			for (int i = 0; i < xattributes.Count; ++i)
			{
				OdDgXAttribute pXAttribute = (OdDgXAttribute)xattributes[i];
				OdBinaryData data = new OdBinaryData();
				pXAttribute.writeData(data);

				UInt32 handlerId = pXAttribute.getHandlerId();
				pDumper.writeFieldValueHex("XAttribute", handlerId);
				pDumper.writeFieldValue("  Class name", pXAttribute.isA().name());
				pDumper.writeFieldValue("  Raw Data Size", data.Count);

				OdDgDgnLinkNodeXAttribute pLink = OdDgDgnLinkNodeXAttribute.cast(pXAttribute);
				OdDgProcessedXmlXAttribute pExtXml = OdDgProcessedXmlXAttribute.cast(pXAttribute);
				OdDgXmlXAttribute pXml = OdDgXmlXAttribute.cast(pXAttribute);

				if (pLink != null && pLink.getLinkNode() != null)
				{
					pDumper.writeFieldValue("  ", pLink.getLinkNode());
					pDumper.writeFieldValue("  ParentAttrId", pLink.getParentAttrId());

					if (pLink.getChildCount() > 0)
					{
						for (UInt32 uChild = 0; uChild < pLink.getChildCount(); uChild++)
						{
							String strFildName;
							strFildName = String.Format("     ChildAttrId %d", uChild);
							pDumper.writeFieldValue(strFildName, pLink.getChildAttrId(uChild));
						}
					}
				}
				else if (pExtXml != null)
				{

					pDumper.writeFieldValue("  " + pExtXml.getRootNode().getName() + ":", pExtXml.getRootNode());
				}
				else if (pXml != null)
				{
					pDumper.writeFieldValue("  Xml", pXml.getXmlString());
				}

				OdDgDetailingSymbolXAttribute pDSXAttr = OdDgDetailingSymbolXAttribute.cast(pXAttribute);

				if (pDSXAttr != null)
				{
					if (pDSXAttr.getXAttributeType() == OdDgDetailingSymbolXAttribute.OdDgDetailingXAttributeType.kSymbolSymbologyFlags)
					{
						String strName = ("kSymbology");

						pDumper.writeFieldValue(("  XAttributeType"), strName);

						if (pDSXAttr.getBubbleSizeFlag())
						{
							pDumper.writeFieldValue(("  BubbleSize"), pDSXAttr.getBubbleSize());
						}
					}
					else
					{
						String strName = ("kProperties");

						pDumper.writeFieldValue(("  XAttributeType"), strName);
						pDumper.writeFieldValue(("  AnnotationScaleFlag"), pDSXAttr.getAnnotationScaleFlag());
						pDumper.writeFieldValue(("  AnnotationScale"), pDSXAttr.getAnnotationScale());
						pDumper.writeFieldValue(("  SymbolSize"), pDSXAttr.getDetailingSymbolSize());
						pDumper.writeFieldValue(("  DetailSymbolArea"), pDSXAttr.getDetailSymbolArea());
					}
				}

				OdDgBinXMLXAttribute pBinXml = OdDgBinXMLXAttribute.cast(pXAttribute);

				if (pBinXml != null)
				{
					pDumper.writeFieldValue("  TypeDescription", pBinXml.getTypeString());
					pDumper.dumpFieldName("  Fields:");

					String strFieldName;
					String strFieldValue;

					for (UInt32 j = 0; j < pBinXml.getRecordCount(); j++)
					{
						OdDgBinXMLXAttribute.OdDgBinXMLParam curRecord = new OdDgBinXMLXAttribute.OdDgBinXMLParam();
						pBinXml.getRecord(j, curRecord);

						for (Int32 k = 0; k < curRecord.m_strPath.Count; k++)
						{
							strFieldName = "";
							strFieldValue = "";

							for (UInt32 n = 0; n < 3 + 2 * k; n++)
							{
								strFieldName += " ";
							}

							if (k == (curRecord.m_strPath.Count - 1))
							{
								if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kString) != 0)
								{
									if (curRecord.m_value.m_varString != "")
									{
										strFieldValue = curRecord.m_value.m_varString;
									}
									else
									{
										strFieldValue = "(none)";
									}
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kBool) != 0)
								{
									strFieldValue = String.Format("{0}", curRecord.m_value.m_varBool);
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kByte) != 0)
								{
									strFieldValue = String.Format("{0}", curRecord.m_value.m_varByte);
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kShort) != 0)
								{
									strFieldValue = String.Format("{0}", curRecord.m_value.m_varShort);
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kDouble) != 0)
								{
									strFieldValue = String.Format("{0}", curRecord.m_value.m_varDouble);
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kPoint3d) != 0)
								{
									strFieldValue = String.Format("{0}, {1}, {2}", curRecord.m_value.m_varPoint3d.x, curRecord.m_value.m_varPoint3d.y, curRecord.m_value.m_varPoint3d.z);
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kColorFlags)
									== curRecord.m_value.m_varColorFlags)
								{
									if (curRecord.m_value.m_varColorFlags == 1)
									{
										strFieldValue = "ByLevel";
									}
									else if (curRecord.m_value.m_varColorFlags == 2)
									{
										strFieldValue = "ByCell";
									}
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kColorBookName) != 0)
								{
									strFieldValue = String.Format("( {0}, {1} )", curRecord.m_value.m_varColorBookIndex, curRecord.m_value.m_varColorIndexInBook);
									strFieldValue = "ColorBook: " + curRecord.m_value.m_varColorBookName + " " + strFieldValue;
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kRGB) != 0)
								{
									strFieldValue = String.Format("RGB : {0:X8}", curRecord.m_value.m_varRGB);
								}
								else if ((curRecord.m_value.m_uFlags & (uint)OdDgBECXMLVariant.OdDgBECXMLVariantFlags.kColorIndex) != 0)
								{
									strFieldValue = String.Format("ColorIndex : {0}", curRecord.m_value.m_varColorIndex);
								}

								strFieldName += "  ";
							}
							else
							{
								strFieldName += "- ";
							}

							strFieldName += curRecord.m_strPath[k];

							if (strFieldValue == "")
							{
								pDumper.dumpFieldName(strFieldName);
							}
							else
							{
								pDumper.writeFieldValue(strFieldName, strFieldValue);
							}
						}
					}
				}

				OdDgTemplateHeaderXAttribute pTmpHdr = OdDgTemplateHeaderXAttribute.cast(pXAttribute);

				if (pTmpHdr != null)
				{
					pDumper.writeFieldValue("  Template Order", pTmpHdr.getTemplateOrder());

					if (pTmpHdr.getTemplateType() == OdDgTemplateHeaderXAttribute.OdDgTemplateHeaderType.kTemplateGroup)
					{
						pDumper.writeFieldValue("  Template Type", "kTemplateGroup");
					}
					else
					{
						pDumper.writeFieldValue("  Template Type", "kElementTemplate");
					}

					pDumper.writeFieldValue("  Template Name", pTmpHdr.getTemplateName());
					pDumper.writeFieldValueHex("  Template ParentId", pTmpHdr.getParentId());
				}

				OdDgSectionClipDataXAttribute pSecClipData = OdDgSectionClipDataXAttribute.cast(pXAttribute);

				if (pSecClipData != null)
				{
					pDumper.writeFieldValue("  Perspective Up Flag", pSecClipData.getPerspectiveUpFlag());
					pDumper.writeFieldValue("  Left Crop Flag", pSecClipData.getLeftCropFlag());
					pDumper.writeFieldValue("  Right Crop Flag", pSecClipData.getRightCropFlag());
					pDumper.writeFieldValue("  Top Crop Flag", pSecClipData.getTopCropFlag());
					pDumper.writeFieldValue("  Bottom Crop Flag", pSecClipData.getBottomCropFlag());
					pDumper.writeFieldValue("  Front Crop Flag", pSecClipData.getFrontCropFlag());
					pDumper.writeFieldValue("  Back Crop Flag", pSecClipData.getBackCropFlag());
					pDumper.writeFieldValue("  Front Depth", pSecClipData.getFrontDepth());
					pDumper.writeFieldValue("  Back  Depth", pSecClipData.getBackDepth());
					pDumper.writeFieldValue("  Top Height", pSecClipData.getTopHeight());
					pDumper.writeFieldValue("  Bottom Height", pSecClipData.getBottomHeight());
					pDumper.writeFieldValue("  Rotation", pSecClipData.getRotation());
				}

				OdDgSectionClipDataExtXAttribute pSecClipDataExt = OdDgSectionClipDataExtXAttribute.cast(pXAttribute);

				if (pSecClipDataExt != null)
				{
					pDumper.writeFieldValue("  Origin", pSecClipDataExt.getOrigin());
					pDumper.writeFieldValue("  Direction Point", pSecClipDataExt.getDirection());
				}

				OdDgTemplateIdXAttribute pTmpIdAttr = OdDgTemplateIdXAttribute.cast(pXAttribute);

				if (pTmpIdAttr != null)
				{
					pDumper.writeFieldValueHex("  Element Template Id", pTmpIdAttr.getTemplateId());
				}

				//pDumper.writeFieldValue("  Raw Data Size", (UInt32)data.Count);

				OdDgRasterClipXAttribute pRasterClipXAttr = OdDgRasterClipXAttribute.cast(pXAttribute);

				if (pRasterClipXAttr != null)
				{
					for (UInt32 n = 0; n < pRasterClipXAttr.getClipElementCount(); n++)
					{
						OdDgRasterClipXAttribute.OdDgRasterClipElementData clipData = pRasterClipXAttr.getClipElementData(n);

						//String strElementName = "";
						String strFieldName;

						strFieldName = String.Format("  Clip element {0}", n);

						//if (clipData.pContourElement != null)
						//{
						//	//swigtype-- SWIG ERROR
						//	strElementName = String.Format("<%ls>", clipData.pContourElement.isA().name().c_str());
						//}
						//else
						//{
						//	strElementName = "<empty>";
						//}

						//pDumper.writeFieldValue(strFieldName, strElementName);
					}
				}

				OdDgAnnotationCellXAttribute pAnnotationCellAttr = OdDgAnnotationCellXAttribute.cast(pXAttribute);

				if (pAnnotationCellAttr != null)
				{
					pDumper.writeFieldValue("  Annotation Scale", pAnnotationCellAttr.getAnnotationScale());
				}

				OdDgDisplayStyleNumberXAttribute pDSNumAttr = OdDgDisplayStyleNumberXAttribute.cast(pXAttribute);

				if (pDSNumAttr != null)
				{
					pDumper.writeFieldValue("  Display Style Entry Id", pDSNumAttr.getDisplayStyleEntryId());
				}

				OdDgClipVolumeSettingsXAttribute pClipVolumeAttr = OdDgClipVolumeSettingsXAttribute.cast(pXAttribute);

				if (pClipVolumeAttr != null)
				{
					pDumper.writeFieldValue("  Forward Display Flag", pClipVolumeAttr.getForwardDisplayFlag());
					pDumper.writeFieldValue("  Forward Snap Flag", pClipVolumeAttr.getForwardSnapFlag());
					pDumper.writeFieldValue("  Forward Locate Flag", pClipVolumeAttr.getForwardLocateFlag());
					pDumper.writeFieldValueHex("  Forward Display Style Id", pClipVolumeAttr.getForwardDisplayStyleEntryId());
					pDumper.writeFieldValue("  Back Display Flag", pClipVolumeAttr.getBackDisplayFlag());
					pDumper.writeFieldValue("  Back Snap Flag", pClipVolumeAttr.getBackSnapFlag());
					pDumper.writeFieldValue("  Back Locate Flag", pClipVolumeAttr.getBackLocateFlag());
					pDumper.writeFieldValueHex("  Back Display Style Id", pClipVolumeAttr.getBackDisplayStyleEntryId());
					pDumper.writeFieldValue("  Cut Display Flag", pClipVolumeAttr.getCutDisplayFlag());
					pDumper.writeFieldValue("  Cut Snap Flag", pClipVolumeAttr.getCutSnapFlag());
					pDumper.writeFieldValue("  Cut Locate Flag", pClipVolumeAttr.getCutLocateFlag());
					pDumper.writeFieldValueHex("  Cut Display Style Id", pClipVolumeAttr.getCutDisplayStyleEntryId());
					pDumper.writeFieldValue("  Outside Display Flag", pClipVolumeAttr.getOutsideDisplayFlag());
					pDumper.writeFieldValue("  Outside Snap Flag", pClipVolumeAttr.getOutsideSnapFlag());
					pDumper.writeFieldValue("  Outside Locate Flag", pClipVolumeAttr.getOutsideLocateFlag());
					pDumper.writeFieldValueHex("  Outside Display Style Id", pClipVolumeAttr.getOutsideDisplayStyleEntryId());
				}

				OdDgDisplayStyleAzimuthAltitudeXAttribute pAzimuthAltitudeXAttr = OdDgDisplayStyleAzimuthAltitudeXAttribute.cast(pXAttribute);
				OdDgDisplayStyleLegendXAttribute pDisplayStyleLegendXAttr = OdDgDisplayStyleLegendXAttribute.cast(pXAttribute);
				OdDgDisplayStyleTypeFilterXAttribute pDisplayStyleTypeFilterXAttr = OdDgDisplayStyleTypeFilterXAttribute.cast(pXAttribute);
				OdDgZippedXAttribute pZippedXAttr = OdDgZippedXAttribute.cast(pXAttribute);

				if (pAzimuthAltitudeXAttr != null)
				{
					pDumper.writeFieldValue("  Azimuth", pAzimuthAltitudeXAttr.getAzimuth());
					pDumper.writeFieldValue("  Altitude", pAzimuthAltitudeXAttr.getAltitude());
				}
				else if (pDisplayStyleTypeFilterXAttr != null)
				{
					pDumper.writeFieldValueHex("  Flags", pDisplayStyleTypeFilterXAttr.getFlags());
					pDumper.writeFieldValue("  ApplyTo", pDisplayStyleTypeFilterXAttr.getApplyToMode());

					if (pDisplayStyleTypeFilterXAttr.getSelectedElementTypeCount() > 0)
					{
						pDumper.dumpFieldName("  Selected element types:");

						for (UInt32 n = 0; n < pDisplayStyleTypeFilterXAttr.getSelectedElementTypeCount(); n++)
						{
							String strFieldName;
							strFieldName = String.Format("    Element Type %d", n);
							pDumper.writeFieldValue(strFieldName, pDisplayStyleTypeFilterXAttr.getSelectedElementType(n));
						}
					}
				}
				else if (pDisplayStyleLegendXAttr != null)
				{
					pDumper.writeFieldValue("  CoordinateAxis", pDisplayStyleLegendXAttr.getCoordinateAxis());
					pDumper.writeFieldValue("  SlopeDisplayMode", pDisplayStyleLegendXAttr.getSlopeDisplayMode());
					pDumper.writeFieldValue("  LegendColorScheme", pDisplayStyleLegendXAttr.getLegendColorScheme());
					pDumper.writeFieldValue("  UseFixedDisplayMinFlag", pDisplayStyleLegendXAttr.getUseFixedDisplayMinFlag());
					pDumper.writeFieldValue("  UseFixedDisplayMaxFlag", pDisplayStyleLegendXAttr.getUseFixedDisplayMaxFlag());
					pDumper.writeFieldValue("  EdgeOverride", pDisplayStyleLegendXAttr.getEdgeOverride());
					pDumper.writeFieldValue("  ValuesCalcMethod", pDisplayStyleLegendXAttr.getValuesCalcMethod());
					pDumper.writeFieldValue("  DisplayLegendFlag", pDisplayStyleLegendXAttr.getDisplayLegendFlag());
					pDumper.writeFieldValue("  SteppedDisplayMethod", pDisplayStyleLegendXAttr.getSteppedDisplayMethod());
					pDumper.writeFieldValue("  TransparentMarginsFlag", pDisplayStyleLegendXAttr.getTransparentMarginsFlag());
					pDumper.writeFieldValue("  DescendingLegendOrderFlag", pDisplayStyleLegendXAttr.getDescendingLegendOrderFlag());
					pDumper.writeFieldValue("  FixedDisplayMinValue", pDisplayStyleLegendXAttr.getFixedDisplayMinValue());
					pDumper.writeFieldValue("  FixedDisplayMaxValue", pDisplayStyleLegendXAttr.getFixedDisplayMaxValue());
					pDumper.writeFieldValue("  LegendStepValue", pDisplayStyleLegendXAttr.getLegendStepValue());
					pDumper.writeFieldValueHex("  MarginColor", pDisplayStyleLegendXAttr.getMarginColor());
					pDumper.writeFieldValue("  LegendTransparency", pDisplayStyleLegendXAttr.getLegendTransparency());

					if (pDisplayStyleLegendXAttr.getColorSchemeItemCount() != 0)
					{
						pDumper.dumpFieldName("  Color Scheme:");

						for (UInt32 n = 0; n < pDisplayStyleLegendXAttr.getColorSchemeItemCount(); n++)
						{
							String strFieldName;
							strFieldName = String.Format("    Item %d", n);
							pDumper.writeFieldValue(strFieldName, pDisplayStyleLegendXAttr.getColorSchemeItem(n));
						}
					}

					if (pDisplayStyleLegendXAttr.getLegendItemCount() != 0)
					{
						pDumper.dumpFieldName("  Legend:");

						for (UInt32 m = 0; m < pDisplayStyleLegendXAttr.getLegendItemCount(); m++)
						{
							String strFieldName;
							strFieldName = String.Format("    Item {0}", m);
							pDumper.writeFieldValue(strFieldName, pDisplayStyleLegendXAttr.getLegendItem(m));
						}
					}
				}
				else if (pZippedXAttr != null)
				{
					OdBinaryData binData = new OdBinaryData();

					pZippedXAttr.getUnzippedData(binData);

					dumpBinaryData(binData, pDumper);
				}

				OdDgProxyXAttribute pProxyXAttrAttr = OdDgProxyXAttribute.cast(pXAttribute);

				if (pProxyXAttrAttr != null)
				{
					OdBinaryData binData = new OdBinaryData();

					pProxyXAttrAttr.getData(binData);

					dumpBinaryData(binData, pDumper);
				}
			}
		}
	}

	protected virtual void dumpReactors(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		OdDgElement pElm = (OdDgElement)pObj;

		OdDgElementIdArray arrPersReactors = new OdDgElementIdArray();
		OdDgElementReactorArray arrTransReactors = new OdDgElementReactorArray();
		pElm.getPersistentReactors(arrPersReactors);
		pElm.getTransientReactors(arrTransReactors);

		pDumper.writeFieldValue("NumPersistentReactors", (UInt32)(arrPersReactors.Count));
		pDumper.writeFieldValue("NumTransientReactors", (UInt32)(arrTransReactors.Count));

		for (Int32 i = 0; i < arrTransReactors.Count; i++)
		{
			OdDgTagAssociationReactor pTagReactor = (OdDgTagAssociationReactor)(arrTransReactors[i]);

			if (pTagReactor != null)
			{
				pDumper.dumpFieldName(" - Tag Association Reactor ");
				pDumper.writeFieldValueHex("     Tag Id", (UInt64)(pTagReactor.getTagElementId().getHandle()));
			}
		}
	}
}

//----------------------------------------------------------
//
// OdDgDumper
//
//----------------------------------------------------------
class OdDgDumper
{
	OdDgElementDumperPE m_elementDumper = new OdDgElementDumperPE();
	OdDgDatabaseDumperPE m_databaseDumper = new OdDgDatabaseDumperPE();

	// tables
	OdDgModelTableDumperPE m_modelTableDumper = new OdDgModelTableDumperPE();
	OdDgModelDumperPE m_modelDumper = new OdDgModelDumperPE();
	OdDgTableDumperPE m_tableDumper = new OdDgTableDumperPE();
	OdDgFontTableDumperPE m_fontTableDumper = new OdDgFontTableDumperPE();
	OdDgLevelFilterTableDumperPE m_levelFilterTableDumper = new OdDgLevelFilterTableDumperPE();
	OdDgLineStyleTableDumperPE m_lineStyleTableDumper = new OdDgLineStyleTableDumperPE();
	OdDgViewGroupTableDumperPE m_viewGroupTableDumper = new OdDgViewGroupTableDumperPE();
	OdDgNamedViewTableDumperPE m_namedViewTableDumper = new OdDgNamedViewTableDumperPE();
	OdDgSharedCellDefinitionTableDumperPE m_sharedCellDefinitionTableDumper = new OdDgSharedCellDefinitionTableDumperPE();
	OdDgTagDefinitionSetTableDumperPE m_tagSetDefinitionTableDumper = new OdDgTagDefinitionSetTableDumperPE();
	OdDgColorTableDumperPE m_colorTableDumper = new OdDgColorTableDumperPE();
	OdDgElementTemplateTableDumperPE m_elementTemplateTableDumper = new OdDgElementTemplateTableDumperPE();
	OdDgNonModelElementCollectionPE m_nmElementCollection = new OdDgNonModelElementCollectionPE();

	OdDgTableRecordDumperPE m_tableRecordDumper = new OdDgTableRecordDumperPE();
	OdDgLevelTableRecordDumperPE m_levelTableRecordDumper = new OdDgLevelTableRecordDumperPE();
	OdDgFontTableRecordDumperPE m_fontTableRecordDumper = new OdDgFontTableRecordDumperPE();
	OdDgTextStyleTableRecordDumperPE m_textStyleTableRecordDumper = new OdDgTextStyleTableRecordDumperPE();
	OdDgDimStyleTableRecordDumperPE m_dimStyleTableRecordDumper = new OdDgDimStyleTableRecordDumperPE();
	OdDgMaterialTableRecordDumperPE m_materialTableRecordDumper = new OdDgMaterialTableRecordDumperPE();
	OdDgMultilineStyleTableRecordDumperPE m_multilineStyleTableRecordDumper = new OdDgMultilineStyleTableRecordDumperPE();
	OdDgLineStyleTableRecordDumperPE m_lineStyleTableRecordDumper = new OdDgLineStyleTableRecordDumperPE();
	OdDgLineStyleDefTableRecordDumperPE m_lineStyleDefsTableRecordDumper = new OdDgLineStyleDefTableRecordDumperPE();
	OdDgLevelFilterTableRecordDumperPE m_levelFilterTableRecordDumper = new OdDgLevelFilterTableRecordDumperPE();
	OdDgDictionaryTableRecordDumperPE m_dicTableRecordDumper = new OdDgDictionaryTableRecordDumperPE();
	OdDgRegAppTableRecordDumperPE m_regAppTableRecordDumper = new OdDgRegAppTableRecordDumperPE();
	OdDgViewGroupDumperPE m_viewGroupDumper = new OdDgViewGroupDumperPE();
	OdDgViewDumperPE m_viewDumper = new OdDgViewDumperPE();
	OdDgLevelMaskDumperPE m_levelMaskDumper = new OdDgLevelMaskDumperPE();
	OdDgSharedCellDefinitionDumperPE m_sharedCellDefinitionDumper = new OdDgSharedCellDefinitionDumperPE();
	OdDgSharedCellReferenceDumperPE m_sharedCellReferenceDumper = new OdDgSharedCellReferenceDumperPE();
	OdDgTagDefinitionSetDumperPE m_tagSetDefinitionDumper = new OdDgTagDefinitionSetDumperPE();
	OdDgTagDefinitionDumperPE m_tagDefinitionDumper = new OdDgTagDefinitionDumperPE();

	OdDgApplicationDataDumperPE m_appDataDumper = new OdDgApplicationDataDumperPE();

	//
	OdDgReferenceAttachmentHeaderDumperPE m_referenceAttachmentDumper = new OdDgReferenceAttachmentHeaderDumperPE();
	OdDgRasterAttachmentHeaderDumperPE m_rasterAttachmentHeaderDumper = new OdDgRasterAttachmentHeaderDumperPE();

	//
	OdDgRasterComponentDumperPE m_rasterComponentDumper = new OdDgRasterComponentDumperPE();

	// graphics elements
	OdDgLine2dDumperPE m_line2dDumper = new OdDgLine2dDumperPE();
	OdDgLine3dDumperPE m_line3dDumper = new OdDgLine3dDumperPE();
	OdDgLineString2dDumperPE m_lineString2dDumper = new OdDgLineString2dDumperPE();
	OdDgLineString3dDumperPE m_lineString3dDumper = new OdDgLineString3dDumperPE();
	OdDgText2dDumperPE m_text2dDumper = new OdDgText2dDumperPE();
	OdDgText3dDumperPE m_text3dDumper = new OdDgText3dDumperPE();
	OdDgShape2dDumperPE m_shape2dDumper = new OdDgShape2dDumperPE();
	OdDgShape3dDumperPE m_shape3dDumper = new OdDgShape3dDumperPE();
	OdDgCurve2dDumperPE m_curve2dDumper = new OdDgCurve2dDumperPE();
	OdDgCurve3dDumperPE m_curve3dDumper = new OdDgCurve3dDumperPE();
	OdDgEllipse2dDumperPE m_ellipse2dDumper = new OdDgEllipse2dDumperPE();
	OdDgEllipse3dDumperPE m_ellipse3dDumper = new OdDgEllipse3dDumperPE();
	OdDgArc2dDumperPE m_arc2dDumper = new OdDgArc2dDumperPE();
	OdDgArc3dDumperPE m_arc3dDumper = new OdDgArc3dDumperPE();
	OdDgConeDumperPE m_coneDumper = new OdDgConeDumperPE();
	OdDgPointString2dDumperPE m_pointString2dDumper = new OdDgPointString2dDumperPE();
	OdDgPointString3dDumperPE m_pointString3dDumper = new OdDgPointString3dDumperPE();
	OdDgDimensionDumperPE m_dimensionDumper = new OdDgDimensionDumperPE();
	OdDgMultilineDumperPE m_multilineDumper = new OdDgMultilineDumperPE();
	OdDgTagElementDumperPE m_tagElementDumper = new OdDgTagElementDumperPE();

	// complex graphics elements
	OdDgBSplineCurve2dDumperPE m_bSplineCurve2dDumper = new OdDgBSplineCurve2dDumperPE();
	OdDgBSplineCurve3dDumperPE m_bSplineCurve3dDumper = new OdDgBSplineCurve3dDumperPE();
	OdDgBSplineSurfaceDumperPE m_bSplineSurfaceDumper = new OdDgBSplineSurfaceDumperPE();
	OdDgCellHeader2dDumperPE m_cellHeader2dDumper = new OdDgCellHeader2dDumperPE();
	OdDgCellHeader3dDumperPE m_cellHeader3dDumper = new OdDgCellHeader3dDumperPE();
	OdDgDistantLightDumperPE m_distantLightDumperPE = new OdDgDistantLightDumperPE();
	OdDgPointLightDumperPE m_pointLightDumperPE = new OdDgPointLightDumperPE();
	OdDgSpotLightDumperPE m_spotLightDumperPE = new OdDgSpotLightDumperPE();
	OdDgAreaLightDumperPE m_areaLightDumperPE = new OdDgAreaLightDumperPE();
	OdDgSkyOpeningLightDumperPE m_skyOpeningLightDumperPE = new OdDgSkyOpeningLightDumperPE();
	OdDgComplexStringDumperPE m_complexStringDumper = new OdDgComplexStringDumperPE();
	OdDgComplexShapeDumperPE m_complexShapeDumper = new OdDgComplexShapeDumperPE();
	OdDgRasterHeader2dDumperPE m_rasterHeader2dDumper = new OdDgRasterHeader2dDumperPE();
	OdDgRasterHeader3dDumperPE m_rasterHeader3dDumper = new OdDgRasterHeader3dDumperPE();
	OdDgTextNode2dDumperPE m_textNode2dDumper = new OdDgTextNode2dDumperPE();
	OdDgTextNode3dDumperPE m_textNode3dDumper = new OdDgTextNode3dDumperPE();
	OdDgSurfaceDumperPE m_surfaceDumper = new OdDgSurfaceDumperPE();
	OdDgSolidDumperPE m_solidDumper = new OdDgSolidDumperPE();

	OdDgProxyElementDumperPE m_proxyDumper = new OdDgProxyElementDumperPE();
	OdDgComplexProxyElementDumperPE m_complexProxyDumper = new OdDgComplexProxyElementDumperPE();

	OdDgCompoundLineStyleResourceDumperPE m_compoundLsResDumper = new OdDgCompoundLineStyleResourceDumperPE();
	OdDgLineCodeResourceDumperPE m_lineCodeLsResDumper = new OdDgLineCodeResourceDumperPE();
	OdDgLinePointResourceDumperPE m_linePointLsResDumper = new OdDgLinePointResourceDumperPE();
	OdDgPointSymbolResourceDumperPE m_pointSymbolLsResDumper = new OdDgPointSymbolResourceDumperPE();

	OdDgLinearAssociationDumperPE m_LinearAssociationDumper = new OdDgLinearAssociationDumperPE();
	OdDgIntersectAssociationDumperPE m_IntersectAssociationDumper = new OdDgIntersectAssociationDumperPE();
	OdDgArcAssociationDumperPE m_ArcAssociationDumper = new OdDgArcAssociationDumperPE();
	OdDgMultilineAssociationDumperPE m_MultilineAssociationDumper = new OdDgMultilineAssociationDumperPE();
	OdDgBSplineCurveAssociationDumperPE m_BSplineCurveAssociationDumper = new OdDgBSplineCurveAssociationDumperPE();
	OdDgProjectionAssociationDumperPE m_ProjectionAssociationDumper = new OdDgProjectionAssociationDumperPE();
	OdDgOriginAssociationDumperPE m_OriginAssociationDumper = new OdDgOriginAssociationDumperPE();
	OdDgIntersect2AssociationDumperPE m_Intersect2AssociationDumper = new OdDgIntersect2AssociationDumperPE();
	OdDgMeshVertexAssociationDumperPE m_MeshVertexAssociationDumper = new OdDgMeshVertexAssociationDumperPE();
	OdDgMeshEdgeAssociationDumperPE m_MeshEdgeAssociationDumper = new OdDgMeshEdgeAssociationDumperPE();
	OdDgBSplineSurfaceAssociationDumperPE m_BSplineSurfaceAssociationDumper = new OdDgBSplineSurfaceAssociationDumperPE();
	OdDgUnknownAssociationDumperPE m_UnknownAssociationDumper = new OdDgUnknownAssociationDumperPE();

	OdDgMeshFaceLoopsDumperPE m_meshFaceLoopsDumper = new OdDgMeshFaceLoopsDumperPE();

	static OdaDgnSampleDrawableOverrule m_SampleDrawableOverrule = new OdaDgnSampleDrawableOverrule();

	public OdDgDumper()
	{
		bool bIsOverruling = false; // true;
		if (bIsOverruling)
		{
			OdRxOverrule.setIsOverruling(true);
		}
		// skip any OdDgArc3d rendering if isOverruliing is true
		OdRxOverrule.addOverrule(OdDgArc3d.desc(), m_SampleDrawableOverrule);

		OdDgRxObjectDumperPE.registerDumper(OdDgElement.desc().name(), m_elementDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgDatabase.desc().name(), m_databaseDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgModelTable.desc().name(), m_modelTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgModel.desc().name(), m_modelDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgLevelTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLevelFilterTable.desc().name(), m_levelFilterTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgFontTable.desc().name(), m_fontTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTextStyleTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgDimStyleTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMaterialTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMultilineStyleTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineStyleTable.desc().name(), m_lineStyleTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineStyleDefTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgDictionaryTable.desc().name(), m_tableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgRegAppTable.desc().name(), m_tableDumper);

		// NonDBRO collections
		OdDgRxObjectDumperPE.registerDumper(OdDgViewGroupTable.desc().name(), m_viewGroupTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgNamedViewTable.desc().name(), m_namedViewTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgSharedCellDefinitionTable.desc().name(), m_sharedCellDefinitionTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTagDefinitionSetTable.desc().name(), m_tagSetDefinitionTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgColorTable.desc().name(), m_colorTableDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgNonModelElementCollection.desc().name(), m_nmElementCollection);

		// Records
		OdDgRxObjectDumperPE.registerDumper(OdDgLevelTableRecord.desc().name(), m_levelTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLevelFilterTableRecord.desc().name(), m_levelFilterTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgFontTableRecord.desc().name(), m_fontTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTextStyleTableRecord.desc().name(), m_textStyleTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgDimStyleTableRecord.desc().name(), m_dimStyleTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMaterialTableRecord.desc().name(), m_materialTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMultilineStyleTableRecord.desc().name(), m_multilineStyleTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineStyleTableRecord.desc().name(), m_lineStyleTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineStyleDefTableRecord.desc().name(), m_lineStyleDefsTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgDictionaryTableRecord.desc().name(), m_dicTableRecordDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgRegAppTableRecord.desc().name(), m_regAppTableRecordDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgViewGroup.desc().name(), m_viewGroupDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgView.desc().name(), m_viewDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLevelMask.desc().name(), m_levelMaskDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgSharedCellDefinition.desc().name(), m_sharedCellDefinitionDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgSharedCellReference.desc().name(), m_sharedCellReferenceDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTagDefinitionSet.desc().name(), m_tagSetDefinitionDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTagDefinition.desc().name(), m_tagDefinitionDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgApplicationData.desc().name(), m_appDataDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgLine2d.desc().name(), m_line2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLine3d.desc().name(), m_line3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineString2d.desc().name(), m_lineString2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineString3d.desc().name(), m_lineString3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgText2d.desc().name(), m_text2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgText3d.desc().name(), m_text3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTextNode2d.desc().name(), m_textNode2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTextNode3d.desc().name(), m_textNode3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgShape2d.desc().name(), m_shape2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgShape3d.desc().name(), m_shape3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgCurve2d.desc().name(), m_curve2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgCurve3d.desc().name(), m_curve3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgEllipse2d.desc().name(), m_ellipse2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgEllipse3d.desc().name(), m_ellipse3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgArc2d.desc().name(), m_arc2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgArc3d.desc().name(), m_arc3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgCone.desc().name(), m_coneDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgComplexString.desc().name(), m_complexStringDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgComplexShape.desc().name(), m_complexShapeDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgPointString2d.desc().name(), m_pointString2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgPointString3d.desc().name(), m_pointString3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgDimension.desc().name(), m_dimensionDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMultiline.desc().name(), m_multilineDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgBSplineCurve2d.desc().name(), m_bSplineCurve2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgBSplineCurve3d.desc().name(), m_bSplineCurve3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgSurface.desc().name(), m_surfaceDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgSolid.desc().name(), m_solidDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgRasterAttachmentHeader.desc().name(), m_rasterAttachmentHeaderDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgRasterHeader2d.desc().name(), m_rasterHeader2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgRasterHeader3d.desc().name(), m_rasterHeader3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgRasterComponent.desc().name(), m_rasterComponentDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgTagElement.desc().name(), m_tagElementDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgCellHeader2d.desc().name(), m_cellHeader2dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgCellHeader3d.desc().name(), m_cellHeader3dDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLightDistant.desc().name(), m_distantLightDumperPE);
		OdDgRxObjectDumperPE.registerDumper(OdDgLightPoint.desc().name(), m_pointLightDumperPE);
		OdDgRxObjectDumperPE.registerDumper(OdDgLightSpot.desc().name(), m_spotLightDumperPE);
		OdDgRxObjectDumperPE.registerDumper(OdDgLightArea.desc().name(), m_areaLightDumperPE);
		OdDgRxObjectDumperPE.registerDumper(OdDgLightOpenSky.desc().name(), m_skyOpeningLightDumperPE);
		OdDgRxObjectDumperPE.registerDumper(OdDgBSplineSurface.desc().name(), m_bSplineSurfaceDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgReferenceAttachmentHeader.desc().name(), m_referenceAttachmentDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMeshFaceLoops.desc().name(), m_meshFaceLoopsDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgProxyElement.desc().name(), m_proxyDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgComplexProxyElement.desc().name(), m_complexProxyDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgCompoundLineStyleResource.desc().name(), m_compoundLsResDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLineCodeResource.desc().name(), m_lineCodeLsResDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgLinePointResource.desc().name(), m_linePointLsResDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgPointSymbolResource.desc().name(), m_pointSymbolLsResDumper);

		OdDgRxObjectDumperPE.registerDumper(OdDgLinearAssociation.desc().name(), m_LinearAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgIntersectAssociation.desc().name(), m_IntersectAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgArcAssociation.desc().name(), m_ArcAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMultilineAssociation.desc().name(), m_MultilineAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgBSplineCurveAssociation.desc().name(), m_BSplineCurveAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgProjectionAssociation.desc().name(), m_ProjectionAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgOriginAssociation.desc().name(), m_OriginAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgIntersect2Association.desc().name(), m_Intersect2AssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMeshVertexAssociation.desc().name(), m_MeshVertexAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgMeshEdgeAssociation.desc().name(), m_MeshEdgeAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgBSplineSurfaceAssociation.desc().name(), m_BSplineSurfaceAssociationDumper);
		OdDgRxObjectDumperPE.registerDumper(OdDgUnknownAssociation.desc().name(), m_UnknownAssociationDumper);
	}
}
//----------------------------------------------------------
//
// OdDgDatabaseDumperPE
//
//----------------------------------------------------------
class OdDgDatabaseDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		OdDgDatabase pDb = (OdDgDatabase)pObj;

		//simple fields
		pDumper.writeFieldValue("Filename", pDb.getFilename());
		pDumper.writeFieldValue("Major version", pDb.getMajorVersion());
		pDumper.writeFieldValue("Minor version", pDb.getMinorVersion());
		pDumper.writeFieldValue("Original file version", pDb.getOriginalFileVersion());
		pDumper.writeFieldValue("Control", pDb.getControlFlags());
		pDumper.writeFieldValue("Control 1", pDb.getControl1Flags());
		pDumper.writeFieldValue("Fbfdcn", pDb.getLockFlags());
		pDumper.writeFieldValue("Ext locks", pDb.getExtLockFlags());
		pDumper.writeFieldValue("Active angle", pDb.getActAngle());
		pDumper.writeFieldValue("Angle round", pDb.getAngleRnd());
		pDumper.writeFieldValue("X active scale", pDb.getXActScale());
		pDumper.writeFieldValue("Y active scale", pDb.getYActScale());
		pDumper.writeFieldValue("Z active scale", pDb.getZActScale());
		pDumper.writeFieldValue("Round scale", pDb.getRoundScale());
		pDumper.writeFieldValue("Azimuth", pDb.getAzimuth());
		pDumper.writeFieldValue("Low", pDb.getExtMin());
		pDumper.writeFieldValue("High", pDb.getExtMax());
		pDumper.writeFieldValue("Active level", pDb.getActiveLevelEntryId());
		pDumper.writeFieldValueHex("Active line style", pDb.getActiveLineStyleEntryId());
		pDumper.writeFieldValueHex("Active line weight", pDb.getActiveLineWeight());
		pDumper.writeFieldValueHex("Active color index", pDb.getActiveColorIndex());
		pDumper.writeFieldValueHex("Fill color", pDb.getFillColorIndex());
		pDumper.writeFieldValue("Active props", pDb.getActiveProps());
		pDumper.writeFieldValue("Line style", pDb.getLineStyle());
		pDumper.writeFieldValue("Line style scale", pDb.getLineStyleScale());
		pDumper.writeFieldValueHex("Multiline flags", pDb.getMultiLineFlags());
		pDumper.writeFieldValue("Text style ID", pDb.getActiveTextStyleEntryId());
		pDumper.writeFieldValue("Text scale lock", pDb.getTextScaleLock());
		pDumper.writeFieldValue("Active model ID", pDb.getActiveModelId().getHandle().ascii());
		pDumper.writeFieldValue("Angle readout format", pDb.getAngleFormat());
		pDumper.writeFieldValue("Angle readout precision", pDb.getAngleReadoutPrec());
		pDumper.writeFieldValue("Angle readout direction mode", pDb.getAngleReadoutDirectionMode());
		pDumper.writeFieldValue("Angle readout base angle", pDb.getAngleReadoutBaseAngle());
		pDumper.writeFieldValue("Angle readout clockwise", pDb.getAngleReadoutClockwiseFlag());
		pDumper.writeFieldValue("Tentative mode", pDb.getTentativeMode());
		pDumper.writeFieldValue("Tentative sub-mode", pDb.getTentativeSubMode());
		pDumper.writeFieldValue("Keypoint dividend", pDb.getKeyPointDividend());
		pDumper.writeFieldValue("Default snap mode", pDb.getDefaultSnapMode());
		pDumper.writeFieldValue("System class", pDb.getSystemClass());
		pDumper.writeFieldValueHex("DMRSF flag", pDb.getDMRSFlag());
		pDumper.writeFieldValue("DMRS linkage generation mode", pDb.getDMRSLinkageGenerationMode());
		pDumper.writeFieldValueHex("Auto dimensions flag", pDb.getAutoDimFlags());
		pDumper.writeFieldValueHex("Associative lock mask", pDb.getAssocLockMask());
		pDumper.writeFieldValue("Active cell", pDb.getActiveCell());
		pDumper.writeFieldValue("Active term cell", pDb.getActiveTermCell());
		pDumper.writeFieldValue("Active term scale", pDb.getActiveTermScale());
		pDumper.writeFieldValue("Active pattern cell", pDb.getActivePatternCell());
		pDumper.writeFieldValue("Active pattern scale", pDb.getActivePatternScale());
		pDumper.writeFieldValue("Active pattern angle", pDb.getActivePatternAngle());
		pDumper.writeFieldValue("Active pattern angle 2", pDb.getActivePatternAngle2());
		pDumper.writeFieldValue("Active pattern row spacing", pDb.getActivePatternRowSpacing());
		pDumper.writeFieldValue("Active pattern column spacing", pDb.getActivePatternColumnSpacing());
		pDumper.writeFieldValue("Pattern tolerance", pDb.getPatternTolerance());
		pDumper.writeFieldValue("Active point type", pDb.getActivePointType());
		pDumper.writeFieldValue("Active point symbol", pDb.getActivePointSymbol());
		pDumper.writeFieldValue("Active point cell", pDb.getActivePointCell());
		pDumper.writeFieldValue("Area pattern angle", pDb.getAreaPatternAngle());
		pDumper.writeFieldValue("Area pattern row spacing", pDb.getAreaPatternRowSpacing());
		pDumper.writeFieldValue("Area pattern column spacing", pDb.getAreaPatternColumnSpacing());
		pDumper.writeFieldValue("Reserved cell", pDb.getReservedCell());
		pDumper.writeFieldValue("Z range 2D low", pDb.getZRange2dLow());
		pDumper.writeFieldValue("Z range 2D high", pDb.getZRange2dHigh());
		pDumper.writeFieldValue("Stream delta", pDb.getStreamDelta());
		pDumper.writeFieldValue("Stream tolerance", pDb.getStreamTolerance());
		pDumper.writeFieldValue("Angle tolerance", pDb.getAngleTolerance());
		pDumper.writeFieldValue("Area tolerance", pDb.getAreaTolerance());
		pDumper.writeFieldValue("Highlight color", pDb.getHighlightColorIndex());
		pDumper.writeFieldValue("XOR color", pDb.getXorColorIndex());
		pDumper.writeFieldValue("Axis lock angle", pDb.getAxisLockAngle());
		pDumper.writeFieldValue("Axis lock origin", pDb.getAxisLockOrigin());
		pDumper.writeFieldValue("Chamfer distance 1", pDb.getChamferDist1());
		pDumper.writeFieldValue("Chamfer distance 2", pDb.getChamferDist2());
		pDumper.writeFieldValue("Autochain tolerance", pDb.getAutochainTolerance());
		pDumper.writeFieldValue("Consline distance", pDb.getConslineDistance());
		pDumper.writeFieldValue("Arc radius", pDb.getArcRadius());
		pDumper.writeFieldValue("Arc length", pDb.getArcLength());
		pDumper.writeFieldValue("Cone radius 1", pDb.getConeRadius1());
		pDumper.writeFieldValue("Cone radius 2", pDb.getConeRadius2());
		pDumper.writeFieldValue("Polygon radius", pDb.getPolygonRadius());
		pDumper.writeFieldValue("Surrev angle", pDb.getSurrevAngle());
		pDumper.writeFieldValue("Extend distance", pDb.getExtendDistance());
		pDumper.writeFieldValue("Fillet radius", pDb.getFilletRadius());
		pDumper.writeFieldValue("Coppar distance", pDb.getCopparDistance());
		pDumper.writeFieldValue("Array row distance", pDb.getArrayRowDistance());
		pDumper.writeFieldValue("Array column distance", pDb.getArrayColumnDistance());
		pDumper.writeFieldValue("Array fill angle", pDb.getArrayFillAngle());
		pDumper.writeFieldValue("Point distance", pDb.getPointDistance());
		pDumper.writeFieldValue("Polygon edges", pDb.getPolygonEdges());
		pDumper.writeFieldValue("Points between", pDb.getPointsBetween());
		pDumper.writeFieldValue("Array num of items", pDb.getArrayNumItems());
		pDumper.writeFieldValue("Array num of rows", pDb.getArrayNumRows());
		pDumper.writeFieldValue("Array num of columns", pDb.getArrayNumCols());
		pDumper.writeFieldValue("Array rotate", pDb.getArrayRotate());
		pDumper.writeFieldValue("B-spline order", pDb.getBSplineOrder());
		pDumper.writeFieldValue("Display attribute type", pDb.getDispAttrType());
		//  pDumper.writeFieldValueHex( "Render flags", pDb.getRenderFlags() );
		pDumper.writeFieldValue("Latitude", pDb.getLatitude());
		pDumper.writeFieldValue("Longitude", pDb.getLongitude());
		pDumper.writeFieldValue("Solar time", pDb.getSolarTime());
		pDumper.writeFieldValue("Solar year", pDb.getSolarYear());
		pDumper.writeFieldValue("GMT offset", pDb.getGMTOffset());
		pDumper.writeFieldValue("Solar direction", pDb.getSolarDirection());
		pDumper.writeFieldValue("Solar vector override", pDb.getSolarVectorOverride());
		pDumper.writeFieldValue("Solar intensity", pDb.getSolarIntensity());
		pDumper.writeFieldValue("Ambient intensity", pDb.getAmbientIntensity());
		pDumper.writeFieldValue("Flash intensity", pDb.getFlashIntensity());
		pDumper.writeFieldValue("Near depth density", pDb.getNearDepthDensity());
		pDumper.writeFieldValue("Far depth density", pDb.getFarDepthDensity());
		pDumper.writeFieldValue("Near depth distance", pDb.getNearDepthDistance());
		pDumper.writeFieldValue("Haze color", pDb.getHazeColor());
		pDumper.writeFieldValue("Shadow tolerance", pDb.getShadowTolerance());
		pDumper.writeFieldValue("Stroke tolerance", pDb.getStrokeTolerance());
		pDumper.writeFieldValue("Max polygon size", pDb.getMaxPolygonSize());
		pDumper.writeFieldValue("Arc minimum", pDb.getArcMinimum());
		pDumper.writeFieldValue("Exact Hline accuracy", pDb.getExactHLineAccuracy());
		pDumper.writeFieldValue("Exact Hline tolerance", pDb.getExactHLineTolerance());
		pDumper.writeFieldValue("Selection Highlight override", pDb.getSelectionHighlightOverride());
		pDumper.writeFieldValue("Selection Highlight color", pDb.getSelectionHighlightColor());
		pDumper.writeFieldValue("Cell filename", pDb.getCellFileName());
		pDumper.writeFieldValue("Background file", pDb.getBackgroundFile());
		//pDumper.writeFieldValue( "Default model is 3D", pDb.getDefaultModelIs3D() );
		//pDumper.writeFieldValue( "Version", pDb.getVersion() );
		//pDumper.writeFieldValue( "Sub version", pDb.getSubVersion() );
		//pDumper.writeFieldValue( "Format", pDb.getFormat() );
		//pDumper.writeFieldValue( "Highest model ID", pDb.getHighestModelID() );
		pDumper.writeFieldValue("Handseed", pDb.getHandseed().ToUInt64());
		pDumper.writeFieldValue("Last saved time", pDb.getLastSaveTime());
		pDumper.writeFieldValue("Next graphics group", pDb.getNextGraphicGroup());
		//pDumper.writeFieldValue( "Next text node", pDb.getNextTextNode() );
		//pDumper.writeFieldValue( "Original format", pDb.getOriginalFormat() );
		//pDumper.writeFieldValue( "Number of model specific digital signatures", pDb.getModelSpecificDigitalSignatures() );
		//pDumper.writeFieldValue( "Number of file-wide digital signatures", pDb.getFileWideDigitalSignatures() );
		//pDumper.writeFieldValue( "Primary application ID", pDb.getPrimaryApplicationID() );
		pDumper.writeFieldValue("Is persistent", pDb.isPersistent());

		OdDgElementId modelId = pDb.getDefaultModelId();
		if (!modelId.isNull())
		{
			OdDgModel pModel = modelId.safeOpenObject() as OdDgModel;
			if (pModel != null)
			{
				pDumper.writeFieldValue("Default Model Name", pModel.getName());
			}
		}
	}
	protected override void dumpLinkages(OdRxObject pObj, OdExDgnDumper pDumper)
	{
	}

	protected override void dumpXAttributes(OdRxObject pObj, OdExDgnDumper pDumper)
	{
	}
}

class OdDgModelTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgModelTable pComplElm = (OdDgModelTable)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
};

//----------------------------------------------------------
//
// OdDgModelDumperPE
//
//----------------------------------------------------------
class OdDgFontTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgFontTable pFontTable = (OdDgFontTable)pElm;
		return pFontTable.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgFontTable pTable = (OdDgFontTable)pObj;

		pDumper.writeFieldValue("-----------------", "------------------");
		pDumper.writeFieldValue("Rsc Font Id", "Rsc Font Name");
		pDumper.writeFieldValue("-----------------", "------------------");

		for (UInt32 i = 0; i <= 255; i++)
		{
			OdDgFontTableRecord pRec = pTable.getFont(i);

			if (pRec != null)
			{
				String strNumber;
				strNumber = String.Format("  {0}", i);
				pDumper.writeFieldValue(strNumber, pRec.getName());
			}
		}

	}
}
class OdDgModelDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgModel pComplElm = (OdDgModel)pElm;
		return pComplElm.createGraphicsElementsIterator(atBeginning, skipDeleted);
	}
	public OdDgElementIterator createControlElementsIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgModel pComplElm = (OdDgModel)pElm;
		return pComplElm.createControlElementsIterator(atBeginning, skipDeleted);
	}
	public override String getName(OdRxObject pObj)
	{
		OdDgModel pModel = (OdDgModel)pObj;
		return pModel.getName();
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgModel element = (OdDgModel)pObj;

		//storage unit
		{
			OdDgModel.StorageUnitDescription description = new OdDgModel.StorageUnitDescription();
			element.getStorageUnit(description);

			String unitName = "Storage unit ";
			pDumper.writeFieldValue(unitName + "Base", description.m_base == OdDgModel.UnitBase.kMeter ? "kMeter" : "kNone");
			pDumper.writeFieldValue(unitName + "System", description.m_system);
			pDumper.writeFieldValue(unitName + "Nominator", description.m_numerator);
			pDumper.writeFieldValue(unitName + "Denominator", description.m_denominator);
			pDumper.writeFieldValue(unitName + "UORs per Storage", description.m_uorPerStorageUnit);
		}

		//master unit
		{
			OdDgModel.UnitDescription description = new OdDgModel.UnitDescription();
			element.getMasterUnit(description);

			String unitName = "Master unit ";
			pDumper.writeFieldValue(unitName + "Base", description.m_base == OdDgModel.UnitBase.kMeter ? "kMeter" : "kNone");
			pDumper.writeFieldValue(unitName + "System", description.m_system);
			pDumper.writeFieldValue(unitName + "Nominator", description.m_numerator);
			pDumper.writeFieldValue(unitName + "Denominator", description.m_denominator);
			pDumper.writeFieldValue(unitName + "Name", description.m_name);
		}

		//sub unit
		{
			OdDgModel.UnitDescription description = new OdDgModel.UnitDescription();
			element.getSubUnit(description);

			String unitName = "Sub unit ";
			pDumper.writeFieldValue(unitName + "Base", description.m_base == OdDgModel.UnitBase.kMeter ? "kMeter" : "kNone");
			pDumper.writeFieldValue(unitName + "System", description.m_system);
			pDumper.writeFieldValue(unitName + "Nominator", description.m_numerator);
			pDumper.writeFieldValue(unitName + "Denominator", description.m_denominator);
			pDumper.writeFieldValue(unitName + "Name", description.m_name);
		}

		pDumper.writeFieldValue("Working unit", element.getWorkingUnit());
		pDumper.writeFieldValue("Global Origin", element.getGlobalOrigin());

		pDumper.writeFieldValue("Solid Extent", element.getSolidExtent());

		{
			OdGeExtents3d extent = new OdGeExtents3d();

			if (element.getGeomExtents(extent) == OdResult.eOk)
			{
				pDumper.writeFieldValue("Extent", extent);
			}
			else
			{
				pDumper.writeFieldValue("Extent", "invalid value");
			}

			// display grid

			pDumper.writeFieldValue("Grid Master", element.getGridMaster());
			pDumper.writeFieldValue("Grid Reference", element.getGridReference());
			pDumper.writeFieldValue("Grid Aspect", element.getGridRatio());
			pDumper.writeFieldValue("Grid Angle", element.getGridAngle());
			pDumper.writeFieldValue("Grid Origin Base", element.getGridBase());
			pDumper.writeFieldValue("Grid Isometric", element.getIsometricGridFlag());
			pDumper.writeFieldValue("Grid Orientation", element.getGridOrientation());

			// ACS

			pDumper.writeFieldValue("ACS Type", element.getAcsType());
			pDumper.writeFieldValue("ACS Origin", element.getAcsOrigin());
			pDumper.writeFieldValue("ACS Rotation", element.getAcsRotation());
			pDumper.writeFieldValueHex("ACS ElementId", element.getAcsElementId().getHandle().ToUInt64());
		}
	}
}
class OdDgViewGroupTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgViewGroupTable pComplElm = (OdDgViewGroupTable)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
}
class OdDgNamedViewTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgNamedViewTable pComplElm = (OdDgNamedViewTable)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
}
class OdDgSharedCellDefinitionTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgSharedCellDefinitionTable pComplElm = (OdDgSharedCellDefinitionTable)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
}
class OdDgTagDefinitionSetTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgTagDefinitionSetTable pComplElm = (OdDgTagDefinitionSetTable)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
};
class OdDgNonModelElementCollectionPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgNonModelElementCollection pComplElm = (OdDgNonModelElementCollection)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
}

//----------------------------------------------------------
//
// Tables
//
//----------------------------------------------------------
class OdDgTableRecordDumperPE : OdDgElementDumperPE
{
	public override String getName(OdRxObject pObj)
	{
		OdDgTableRecord pRec = (OdDgTableRecord)pObj;
		return pRec.getName();
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgTableRecord pRec = (OdDgTableRecord)pObj;
		pDumper.writeFieldValue("Name ", pRec.getName());
	}
}
class OdDgLevelTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLevelTableRecord pLevel = (OdDgLevelTableRecord)pObj;

		pDumper.writeFieldValue("Level Number", pLevel.getNumber());
		pDumper.writeFieldValue("Entry ID", pLevel.getEntryId());
		pDumper.writeFieldValue("Code", pLevel.getNumber());

		pDumper.writeFieldValueHex("Element Color (byLevel)", pLevel.getElementColorIndex());
		pDumper.writeFieldValueHex("Element LineStyle (byLevel)", pLevel.getElementLineStyleEntryId());
		pDumper.writeFieldValue("Element LineWeight (byLevel)", pLevel.getElementLineWeight());

		pDumper.writeFieldValueHex("Override Color", pLevel.getOverrideColorIndex());
		pDumper.writeFieldValueHex("Override LineStyle", pLevel.getOverrideLineStyleEntryId());
		pDumper.writeFieldValue("Override LineWeight", pLevel.getOverrideLineWeight());
		pDumper.writeFieldValue("Override Material", pLevel.getOverrideMaterial());

		pDumper.writeFieldValue("Use override color", pLevel.getUseOverrideColorFlag());
		pDumper.writeFieldValue("Use override line style", pLevel.getUseOverrideLineStyleFlag());
		pDumper.writeFieldValue("Use override line weight", pLevel.getUseOverrideLineWeightFlag());

		pDumper.writeFieldValue("Displayed", pLevel.getIsDisplayedFlag());
		pDumper.writeFieldValue("Can be Plotted", pLevel.getIsPlotFlag());
		pDumper.writeFieldValue("Derived from a library level ", pLevel.getIsExternalFlag());
		pDumper.writeFieldValue("Can be Snapped ", pLevel.getIsSnapFlag());
		pDumper.writeFieldValue("ReadOnly", pLevel.getIsReadOnlyFlag());
		pDumper.writeFieldValue("Hidden", pLevel.getIsHiddenFlag());
		pDumper.writeFieldValue("Frozen", pLevel.getIsFrozenFlag());
		pDumper.writeFieldValue("CustomStyleFromMaster", pLevel.getIsCustomStyleFromMasterFlag());
		pDumper.writeFieldValue("Displayed", pLevel.getIsDisplayedFlag());
		pDumper.writeFieldValueHex("Element access", (uint)pLevel.getElementAccess());

		pDumper.writeFieldValue("Description", pLevel.getDescription());
		pDumper.writeFieldValue("Priority", pLevel.getPriority());
		pDumper.writeFieldValue("Transparency", 1.0 - pLevel.getTransparency().alphaPercent());

		if (!pLevel.getOverrideMaterial().isNull())
		{
			pDumper.writeFieldValueHex("Override Material Id", pLevel.getOverrideMaterial().getHandle().ToUInt64());
			pDumper.writeFieldValue("Override Material Palette", pLevel.getOverrideMaterialPalette());
			pDumper.writeFieldValue("Override Material Name", pLevel.getOverrideMaterialName());
		}

		if (!pLevel.getByLevelMaterial().isNull())
		{
			pDumper.writeFieldValueHex("ByLevel Material Id", pLevel.getByLevelMaterial().getHandle().ToUInt64());
			pDumper.writeFieldValue("ByLevel Material Palette", pLevel.getByLevelMaterialPalette());
			pDumper.writeFieldValue("ByLevel Material Name", pLevel.getByLevelMaterialName());
		}

		if (pLevel.getAssignedMaterialsCount() > 0)
		{
			pDumper.dumpFieldName("Assigned materials:");

			for (UInt32 k = 0; k < pLevel.getAssignedMaterialsCount(); k++)
			{
				OdDgAssignedMaterial matPair;
				matPair = pLevel.getAssignedMaterial(k);
				pDumper.writeFieldValue("  " + matPair.m_strMaterialName, matPair.m_uColorIndex);
			}
		}
	}
}
class OdDgLevelFilterTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLevelFilterTableRecord pFilter = (OdDgLevelFilterTableRecord)pObj;

		pDumper.writeFieldValue("Entry ID", pFilter.getEntryId());
		pDumper.writeFieldValue("Parent ID", pFilter.getParentId());
		pDumper.writeFieldValue("Filter Type", pFilter.getFilterType());
		pDumper.writeFieldValue("Compose Or Flag", pFilter.getComposeOrFlag());
		pDumper.dumpFieldName("Filter Expressions:");

		bool bAddData = false;

		for (UInt32 i = 0; i < pFilter.getFilterMemberCount(); i++)
		{
			String strMemberName = pFilter.getFilterMemberName(i);
			String strMemberExpression = pFilter.getFilterMemberExpression(i);

			if (strMemberExpression != "")
			{
				strMemberName = "  " + strMemberName;

				pDumper.writeFieldValue(strMemberName, strMemberExpression);
				bAddData = true;
			}
		}

		if (!bAddData)
		{
			pDumper.dumpFieldValue("-------");
		}
	}
}
class OdDgTableDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgTable pComplElm = (OdDgTable)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
}
class OdDgLineStyleTableDumperPE : OdDgTableDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLineStyleTable pTable = (OdDgLineStyleTable)pObj;
		pDumper.dumpFieldName("Available rsc line styles:");

		for (UInt32 i = 0; i < pTable.getRscLineStyleCount(); i++)
		{
			OdDgLineStyleTableRecord pLineStyle = pTable.getRscLineStyle(i) as OdDgLineStyleTableRecord;

			if (pLineStyle != null)
			{
				String strName = pLineStyle.getName();

				strName = " " + strName;

				pDumper.writeFieldValueHex(strName, pLineStyle.getEntryId());
			}
		}
	}
}
class OdDgLevelFilterTableDumperPE : OdDgTableDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLevelFilterTable pTable = (OdDgLevelFilterTable)pObj;
		pDumper.writeFieldValue("Active Filter ID", pTable.getActiveFilterId());
		pDumper.dumpFieldName("Filter Members :");

		for (UInt32 i = 0; i < pTable.getFilterMemberCount(); i++)
		{
			String strMemberName = "";
			OdDgLevelFilterTable.OdDgFilterMemberType iMemberType;

			if (pTable.getFilterMember(i, ref strMemberName, out iMemberType))
			{
				strMemberName = "  " + strMemberName;
				pDumper.writeFieldValue(strMemberName, iMemberType);
			}
		}
	}
}

class OdDgFontTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgFontTableRecord pFont = (OdDgFontTableRecord)pObj;

		pDumper.writeFieldValue("Font number", pFont.getNumber());
		pDumper.writeFieldValue("Entry ID", pFont.getNumber());
		pDumper.writeFieldValue("Alternate Font name", pFont.getAlternateName());
	}
}
class OdDgTextStyleTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgTextStyleTableRecord element = (OdDgTextStyleTableRecord)pObj;

		pDumper.writeFieldValue("Entry ID", element.getEntryId());

		pDumper.writeFieldValue("Font number", element.getFontEntryId());
		// Gets Font name
		OdDgFontTable pFontTable = element.database().getFontTable();
		OdDgFontTableRecord pFont = pFontTable.getFont(element.getFontEntryId());
		if (pFont != null)
		{
			pDumper.writeFieldValue("Font Name", pFont.getName());
		}

		pDumper.writeFieldValue("Justification", element.getJustification());
		pDumper.writeFieldValue("Line length", element.getLineLength());
		pDumper.writeFieldValue("Line offset", element.getLineOffset());
		pDumper.writeFieldValue("Line spacing", element.getLineSpacing());
		pDumper.writeFieldValue("Font number", element.getFontEntryId());
		pDumper.writeFieldValue("Interchar spacing", element.getInterCharacter());
		pDumper.writeFieldValue("Parent text style ID", element.getParentTextStyleEntryId());
		pDumper.writeFieldValue("Slant", element.getSlant());
		pDumper.writeFieldValue("Direction", element.getTextDirection());
		pDumper.writeFieldValue("Height", element.getTextHeight());
		pDumper.writeFieldValue("Width", element.getTextWidth());
		pDumper.writeFieldValue("Node justification", element.getTextNodeJustification());
		//writeFieldValue( "Is from table", element.getIsFromTableFlag() );

		pDumper.writeFieldValue("Underline", element.getUnderlineFlag());
		pDumper.writeFieldValue("Overline", element.getOverlineFlag());
		pDumper.writeFieldValue("Italic", element.getItalicsFlag());
		pDumper.writeFieldValue("Bold", element.getBoldFlag());
		pDumper.writeFieldValue("Superscript", element.getSuperscriptFlag());
		pDumper.writeFieldValue("Subscript", element.getSubscriptFlag());
		pDumper.writeFieldValue("Background flag", element.getBackgroundFlag());
		pDumper.writeFieldValue("OverlineStyle", element.getOverlineStyleFlag());
		pDumper.writeFieldValue("UnderlineStyle", element.getUnderlineStyleFlag());
		pDumper.writeFieldValue("Fixed spacing", element.getFixedSpacingFlag());
		pDumper.writeFieldValue("Fractions", element.getFractionsFlag());
		pDumper.writeFieldValue("Color flag", element.getColorFlag());
		pDumper.writeFieldValue("AcadIntercharSpacingFlag", element.getAcadIntercharSpacingFlag());
		pDumper.writeFieldValue("FullJustificationFlag", element.getFullJustificationFlag());
		pDumper.writeFieldValue("AcadShapeFileFlag", element.getAcadShapeFileFlag());

		pDumper.writeFieldValue("Background border", element.getBackgroundBorder());
		pDumper.writeFieldValue("Background fill color index", element.getBackgroundFillColorIndex());
		pDumper.writeFieldValue("Background line color index", element.getBackgroundColorIndex());
		pDumper.writeFieldValue("Background line style entry id", element.getBackgroundLineStyleEntryId());
		pDumper.writeFieldValue("Background line weight", element.getBackgroundLineWeight());
		pDumper.writeFieldValue("Overline line color", element.getOverlineColorIndex());
		pDumper.writeFieldValue("Overline line style ID", element.getOverlineLineStyleEntryId());
		pDumper.writeFieldValue("Overline line weight", element.getOverlineLineWeight());
		pDumper.writeFieldValue("Overline offset", element.getOverlineOffset());
		pDumper.writeFieldValue("Underline line color", element.getUnderlineColorIndex());
		pDumper.writeFieldValue("Underline line style ID", element.getUnderlineLineStyleEntryId());
		pDumper.writeFieldValue("Underline line weight", element.getUnderlineLineWeight());
		pDumper.writeFieldValue("Underline offset", element.getUnderlineOffset());
		pDumper.writeFieldValue("LineSpacinType offset", element.getLineSpacingType());
		pDumper.writeFieldValue("Color index", element.getColorIndex());

		pDumper.dumpFieldName("Override flags:");
		pDumper.writeFieldValue(" - FontNumber Override Flag", element.getFontNumberOverrideFlag());
		pDumper.writeFieldValue(" - ShxBigFont Override Flag", element.getShxBigFontOverrideFlag());
		pDumper.writeFieldValue(" - Width Override Flag", element.getWidthOverrideFlag());
		pDumper.writeFieldValue(" - Height Override Flag", element.getHeightOverrideFlag());
		pDumper.writeFieldValue(" - Slant Override Flag", element.getSlantOverrideFlag());
		pDumper.writeFieldValue(" - LineSpacing Override Flag", element.getLineSpacingOverrideFlag());
		pDumper.writeFieldValue(" - LineCharSpacing Override Flag", element.getLineCharSpacingOverrideFlag());
		pDumper.writeFieldValue(" - UnderlineOffset Override Flag", element.getUnderlineOffsetOverrideFlag());
		pDumper.writeFieldValue(" - OverlineOffset Override Flag", element.getOverlineOffsetOverrideFlag());
		pDumper.writeFieldValue(" - Justification Override Flag", element.getJustificationOverrideFlag());
		pDumper.writeFieldValue(" - NodeJustification Override Flag", element.getNodeJustificationOverrideFlag());
		pDumper.writeFieldValue(" - LineLength Override Flag", element.getLineLengthOverrideFlag());
		pDumper.writeFieldValue(" - Direction Override Flag", element.getDirectionOverrideFlag());
		pDumper.writeFieldValue(" - Underline Override Flag", element.getUnderlineOverrideFlag());
		pDumper.writeFieldValue(" - Overline Override Flag", element.getOverlineOverrideFlag());
		pDumper.writeFieldValue(" - Italics Override Flag", element.getItalicsOverrideFlag());
		pDumper.writeFieldValue(" - Bold Override Flag", element.getBoldOverrideFlag());
		pDumper.writeFieldValue(" - Superscript Override Flag", element.getSuperscriptOverrideFlag());
		pDumper.writeFieldValue(" - Subscript Override Flag", element.getSubscriptOverrideFlag());
		pDumper.writeFieldValue(" - FixedSpacing Override Flag", element.getFixedSpacingOverrideFlag());
		pDumper.writeFieldValue(" - Background Override Flag", element.getBackgroundOverrideFlag());
		pDumper.writeFieldValue(" - BackgroundStyle Override Flag", element.getBackgroundStyleOverrideFlag());
		pDumper.writeFieldValue(" - BackgroundWeight Override Flag", element.getBackgroundWeightOverrideFlag());
		pDumper.writeFieldValue(" - BackgroundColor Override Flag", element.getBackgroundColorOverrideFlag());
		pDumper.writeFieldValue(" - BackgroundFillColor Override Flag", element.getBackgroundFillColorOverrideFlag());
		pDumper.writeFieldValue(" - BackgroundBorder Override Flag", element.getBackgroundBorderOverrideFlag());
		pDumper.writeFieldValue(" - UnderlineStyle Override Flag", element.getUnderlineStyleOverrideFlag());
		pDumper.writeFieldValue(" - UnderlineWeight Override Flag", element.getUnderlineWeightOverrideFlag());
		pDumper.writeFieldValue(" - UnderlineColor Override Flag", element.getUnderlineColorOverrideFlag());
		pDumper.writeFieldValue(" - OverlineStyle Override Flag", element.getOverlineStyleOverrideFlag());
		pDumper.writeFieldValue(" - OverlineWeight Override Flag", element.getOverlineWeightOverrideFlag());
		pDumper.writeFieldValue(" - OverlineColor Override Flag", element.getOverlineColorOverrideFlag());
		pDumper.writeFieldValue(" - LineOffset Override Flag", element.getLineOffsetOverrideFlag());
		pDumper.writeFieldValue(" - Fractions Override Flag", element.getFractionsOverrideFlag());
		pDumper.writeFieldValue(" - OverlineStyleFlag Override Flag", element.getOverlineStyleFlagOverrideFlag());
		pDumper.writeFieldValue(" - UnderlineStyleFlag Override Flag", element.getUnderlineStyleFlagOverrideFlag());
		pDumper.writeFieldValue(" - Color Override Flag", element.getColorOverrideFlag());
		pDumper.writeFieldValue(" - WidthFactor Override Flag", element.getWidthFactorOverrideFlag());
		pDumper.writeFieldValue(" - ColorFlag Override Flag", element.getColorFlagOverrideFlag());
		pDumper.writeFieldValue(" - FullJustification Override Flag", element.getFullJustificationOverrideFlag());
		pDumper.writeFieldValue(" - LineSpacingType Override Flag", element.getLineSpacingTypeOverrideFlag());
		pDumper.writeFieldValue(" - Backwards Override Flag", element.getBackwardsOverrideFlag());
		pDumper.writeFieldValue(" - Upsidedown Override Flag", element.getUpsidedownOverrideFlag());
		pDumper.writeFieldValue(" - AcadInterCharSpacing Override Flag", element.getAcadInterCharSpacingOverrideFlag());
	}
}
class OdDgDimStyleTableRecordDumperPE : OdDgTableRecordDumperPE
{
	static void getToolTypeAndDescriptionByToolIndex(UInt32 iIndex, out OdDgDimension.ToolType iTool, out String strToolName)
	{
		switch (iIndex)
		{
			case 0:
				{
					iTool = OdDgDimension.ToolType.kToolTypeSizeArrow;
					strToolName = "  Tool type \"kToolTypeSizeArrow\"";
				}
				break;

			case 1:
				{
					iTool = OdDgDimension.ToolType.kToolTypeSizeStroke;
					strToolName = "  Tool type \"kToolTypeSizeStroke\"";
				}
				break;

			case 2:
				{
					iTool = OdDgDimension.ToolType.kToolTypeLocateSingle;
					strToolName = "  Tool type \"kToolTypeLocateSingle\"";
				}
				break;

			case 3:
				{
					iTool = OdDgDimension.ToolType.kToolTypeLocateStacked;
					strToolName = "  Tool type \"kToolTypeLocateStacked\"";
				}
				break;

			case 4:
				{
					iTool = OdDgDimension.ToolType.kToolTypeAngleSize;
					strToolName = "  Tool type \"kToolTypeAngleSize\"";
				}
				break;

			case 5:
				{
					iTool = OdDgDimension.ToolType.kToolTypeArcSize;
					strToolName = "  Tool type \"kToolTypeArcSize\"";
				}
				break;

			case 6:
				{
					iTool = OdDgDimension.ToolType.kToolTypeAngleLocation;
					strToolName = "  Tool type \"kToolTypeAngleLocation\"";
				}
				break;

			case 7:
				{
					iTool = OdDgDimension.ToolType.kToolTypeArcLocation;
					strToolName = "  Tool type \"kToolTypeArcLocation\"";
				}
				break;

			case 8:
				{
					iTool = OdDgDimension.ToolType.kToolTypeAngleLines;
					strToolName = "  Tool type \"kToolTypeAngleLines\"";
				}
				break;

			case 9:
				{
					iTool = OdDgDimension.ToolType.kToolTypeAngleAxis;
					strToolName = "  Tool type \"kToolTypeAngleAxis\" ( X and Y )";
				}
				break;

			case 10:
				{
					iTool = OdDgDimension.ToolType.kToolTypeRadius;
					strToolName = "  Tool type \"kToolTypeRadius\"";
				}
				break;

			case 11:
				{
					iTool = OdDgDimension.ToolType.kToolTypeRadiusExtended;
					strToolName = "  Tool type \"kToolTypeRadiusExtended\"";
				}
				break;

			case 12:
				{
					iTool = OdDgDimension.ToolType.kToolTypeDiameter;
					strToolName = "  Tool type \"kToolTypeDiameter\"";
				}
				break;

			case 13:
				{
					iTool = OdDgDimension.ToolType.kToolTypeDiameterExtended;
					strToolName = "  Tool type \"kToolTypeDiameterExtended\"";
				}
				break;

			case 14:
				{
					iTool = OdDgDimension.ToolType.kToolTypeDiameterPara;
					strToolName = "  Tool type \"kToolTypeDiameterPara\"";
				}
				break;

			case 15:
				{
					iTool = OdDgDimension.ToolType.kToolTypeDiameterPerp;
					strToolName = "  Tool type \"kToolTypeDiameterPerp\"";
				}
				break;

			case 16:
				{
					iTool = OdDgDimension.ToolType.kToolTypeCustomLinear;
					strToolName = "  Tool type \"kToolTypeCustomLinear\"";
				}
				break;

			case 17:
				{
					iTool = OdDgDimension.ToolType.kToolTypeOrdinate;
					strToolName = "  Tool type \"kToolTypeOrdinate\"";
				}
				break;

			case 18:
				{
					iTool = OdDgDimension.ToolType.kToolTypeCenter;
					strToolName = "  Tool type \"kToolTypeCenter\"";
				}
				break;

			case 19:
				{
					iTool = OdDgDimension.ToolType.kToolTypeLabelLine;
					strToolName = "  Tool type \"kToolTypeLabelLine\"";
				}
				break;

			case 20:
				{
					iTool = OdDgDimension.ToolType.kToolTypeNote;
					strToolName = "  Tool type \"kToolTypeNote\"";
				}
				break;

			default:
				{
					iTool = OdDgDimension.ToolType.kToolTypeInvalid;
					strToolName = "  Invalid Tool type";
				}
				break;
		}
	}

	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgDimStyleTableRecord element = (OdDgDimStyleTableRecord)pObj;

		pDumper.writeFieldValue("Description", element.getDescription());
		pDumper.writeFieldValue("Entry ID", element.getEntryId());
		pDumper.writeFieldValue("Parent ID", element.getParentDimensionStyleEntryId());
		pDumper.writeFieldValue("Level ID", element.getLevel());
		pDumper.writeFieldValue("Text Style ID", element.getTextStyleEntryId());
		pDumper.writeFieldValue("Primary Accuracy", element.getPrimaryAccuracy());
		pDumper.writeFieldValue("Secondary Accuracy", element.getSecondaryAccuracy());
		pDumper.dumpFieldName("Mode Flags:");
		pDumper.writeFieldValue("  Placement alignment", element.getPlacementAlignment());
		pDumper.writeFieldValue("  Manual placement location", element.getManualLocationFlag());
		pDumper.writeFieldValue("  Extension lines present", element.getExtensionLinesPresentFlag());
		pDumper.writeFieldValue("  Text Justification", element.getTextJustification());

		if (element.getShowAngleFormatFlag())
			pDumper.writeFieldValue("  Angle Format Units", "Angle");
		else
			pDumper.writeFieldValue("  Angle Format Units", "Length");

		pDumper.dumpFieldName("Param Flags:");
		pDumper.writeFieldValue("  Hide 0 su for primary units", element.getHideZeroPrimarySubUnitsFlag());
		pDumper.writeFieldValue("  Hide 0 su for secondary units", element.getHideZeroSecondarySubUnitsFlag());
		pDumper.writeFieldValue("  Show 0 mu for primary units", element.getShowZeroPrimaryMasterUnitsFlag());
		pDumper.writeFieldValue("  Show 0 mu for secondary units", element.getShowZeroSecondaryMasterUnitsFlag());
		pDumper.writeFieldValue("  Use working units as primary units", !element.getUseCustomUnitsFlag());
		pDumper.writeFieldValue("  Show secondary units", !element.getShowSecondaryUnitsFlag());
		pDumper.writeFieldValue("  Enable tolerance", element.getShowToleranceFlag());

		if (element.getUseToleranceLimitModeFlag())
			pDumper.writeFieldValue("  Tolerance mode", "Limit");
		else
			pDumper.writeFieldValue("  Tolerance mode", "Plus/Minus");

		pDumper.writeFieldValue("  Embed text", element.getEmbedTextFlag());
		pDumper.writeFieldValue("  Text location", element.getTextLocation());

		if (element.getHorizontalTextFlag())
			pDumper.writeFieldValue("  Text alignment", "Horizontal");
		else
			pDumper.writeFieldValue("  Text alignment", "Aligned");

		pDumper.writeFieldValue("  Primary label display mode", element.getPrimaryLabelDisplayMode());
		pDumper.writeFieldValue("  Secondary label display mode", element.getSecondaryLabelDisplayMode());
		pDumper.writeFieldValue("Arrow symbol font id", element.getArrowFont());
		pDumper.writeFieldValue("Arrow symbol code", element.getArrowChar());
		pDumper.writeFieldValue("Stroke symbol font id", element.getStrokeFont());
		pDumper.writeFieldValue("Stroke symbol code", element.getStrokeChar());
		pDumper.writeFieldValue("Origin symbol font id", element.getOriginFont());
		pDumper.writeFieldValue("Origin symbol code", element.getOriginChar());
		pDumper.writeFieldValue("Diameter symbol font id", element.getDiameterFont());
		pDumper.writeFieldValue("Diameter symbol code", element.getDiameterChar());
		pDumper.writeFieldValue("Text height", element.getTextHeight());
		pDumper.writeFieldValue("Lower tolerance value", element.getLowerToleranceValue());
		pDumper.writeFieldValue("Upper tolerance value", element.getUpperToleranceValue());
		pDumper.writeFieldValue("Tolerance text scale", element.getToleranceTextScale());
		pDumper.writeFieldValue("Witness line offset", element.getWitnessLineOffset());
		pDumper.writeFieldValue("Witness line extension", element.getWitnessLineExtension());
		pDumper.writeFieldValue("Dimension scale", element.getDimensionScale());
		pDumper.writeFieldValue("Geometry Margin", element.getGeometryMargin());
		pDumper.dumpFieldName("Extended Flags:");
		pDumper.writeFieldValue("  Joiner flag", element.getJoinerFlag());
		pDumper.writeFieldValue("  Box text flag", element.getBoxTextFlag());
		pDumper.writeFieldValue("  Semi-Auto placement location flag", element.getSemiAutoFlag());
		pDumper.writeFieldValue("  Primary leading zero flag", element.getPrimaryLeadingZeroFlag());
		pDumper.writeFieldValue("  Primary trailing zeros flag", element.getPrimaryTrailingZerosFlag());
		pDumper.writeFieldValue("  Decimal comma flag", element.getDecimalCommaFlag());
		pDumper.writeFieldValue("  Capsule text flag", element.getCapsuleTextFlag());
		pDumper.writeFieldValue("  Superscript LSD flag", element.getSuperscriptLSDFlag());
		pDumper.writeFieldValue("  Round LSD flag", element.getRoundLSDFlag());
		pDumper.writeFieldValue("  Omit leading delimiter flag", element.getOmitLeadingDelimiterFlag());
		pDumper.writeFieldValue("  Dim lines color override flag", element.getDimLinesColorOverrideFlag());
		pDumper.writeFieldValue("  Dim lines weight override flag", element.getDimLinesWeightOverrideFlag());
		pDumper.writeFieldValue("  Text color override flag", element.getTextColorOverrideFlag());
		pDumper.writeFieldValue("  Text weight override flag", element.getTextWeightOverrideFlag());
		pDumper.writeFieldValue("  Text font override flag", element.getTextFontOverrideFlag());
		pDumper.writeFieldValue("  Level override flag", element.getLevelOverrideFlag());
		pDumper.writeFieldValue("  Text height override flag", element.getTextHeightOverrideFlag());
		pDumper.writeFieldValue("  Drop dimension flag", element.getDropDimensionAfterCraetionFlag());
		pDumper.writeFieldValue("  Terminator Arrow head type", element.getTerminatorArrowHead());
		pDumper.writeFieldValue("  Use reference file scale flag", element.getUseReferenceFileScaleFlag());
		pDumper.writeFieldValue("  Relative dimension lines flag", element.getRelativeDimensionLineFlag());
		pDumper.writeFieldValue("  Underline text flag", element.getTextUnderlineFlag());
		pDumper.writeFieldValue("  Dim lines style override flag", element.getDimLinesStyleOverrideFlag());
		pDumper.writeFieldValue("  No auto text lift flag", element.getNoAutoTextLiftFlag());
		pDumper.writeFieldValue("  Terminator orientation", element.getTerminatorOrientation());
		pDumper.writeFieldValue("  Master file units flag", element.getMasterFileUnitsFlag());
		pDumper.writeFieldValue("  Override level symbology flag", element.getOverrideLevelSymbologyFlag());
		pDumper.writeFieldValue("  View rotation flag", element.getViewRotationFlag());
		pDumper.writeFieldValue("  Secondary leading zero flag", element.getSecondaryLeadingZeroFlag());
		pDumper.writeFieldValue("  Secondary trailing zeros flag", element.getSecondaryTrailingZerosFlag());

		pDumper.dumpFieldName("Tool specific:");

		for (UInt32 i = 0; i < 19; i++)
		{
			OdDgDimension.ToolType iTool;
			String strToolName;

			getToolTypeAndDescriptionByToolIndex(i, out iTool, out strToolName);

			pDumper.dumpFieldName(strToolName);
			pDumper.writeFieldValue("    First terminator", element.getToolTermFirst(iTool));
			pDumper.writeFieldValue("    Left  terminator", element.getToolTermLeft(iTool));
			pDumper.writeFieldValue("    Right terminator", element.getToolTermRight(iTool));
			pDumper.writeFieldValue("    Joint terminator", element.getToolTermJoint(iTool));
			pDumper.writeFieldValue("    Prefix", element.getToolPrefix(iTool));
			pDumper.writeFieldValue("    Stack extension lines flag", element.getToolStackExtLinesFlag(iTool));
			pDumper.writeFieldValue("    Suffix", element.getToolSuffix(iTool));
			pDumper.writeFieldValue("    Show arc symbol flag", element.getToolExtLinesArcFlag(iTool));
			pDumper.writeFieldValue("    Show left extension line flag", element.getToolLeftExtLinesPresentFlag(iTool));
			pDumper.writeFieldValue("    Show right extension line flag", element.getToolRightExtLinesPresentFlag(iTool));
			pDumper.writeFieldValue("    Text type", element.getToolTextType(iTool));
			pDumper.writeFieldValue("    Center mark flag", element.getToolCenterMarkFlag(iTool));
			pDumper.writeFieldValue("    Center mark left extension flag", element.getToolCenterMarkLeftExtendFlag(iTool));
			pDumper.writeFieldValue("    Center mark right extension flag", element.getToolCenterMarkRightExtendFlag(iTool));
			pDumper.writeFieldValue("    Center mark top extension flag", element.getToolCenterMarkTopExtendFlag(iTool));
			pDumper.writeFieldValue("    Center mark bottom extension flag", element.getToolCenterMarkBottomExtendFlag(iTool));
			pDumper.writeFieldValue("    Chord align flag", element.getToolChordAlignFlag(iTool));
		}
		pDumper.writeFieldValue("Stack offset", element.getStackOffset());
		pDumper.writeFieldValue("Center mark size", element.getCenterMarkSize());
		pDumper.writeFieldValue("Current dimension cmd", element.getCurrentDimensionCmd());
		pDumper.writeFieldValue("Angle display mode", element.getAngleDisplayMode());
		pDumper.writeFieldValue("Angle accuracy", element.getAngleAccuracy());
		pDumper.writeFieldValue("Main prefix", element.getMainPrefix());
		pDumper.writeFieldValue("Main suffix", element.getMainSuffix());
		pDumper.writeFieldValue("Tolerance prefix", element.getTolerancePrefix());
		pDumper.writeFieldValue("Tolerance suffix", element.getToleranceSuffix());
		pDumper.writeFieldValue("Upper prefix", element.getUpperPrefix());
		pDumper.writeFieldValue("Upper suffix", element.getUpperSuffix());
		pDumper.writeFieldValue("Lower prefix", element.getLowerPrefix());
		pDumper.writeFieldValue("Lower suffix", element.getLowerSuffix());
		pDumper.writeFieldValue("Dimension color", element.getDimensionColor());
		pDumper.writeFieldValue("Dimension line weight", element.getDimensionLineWeight());
		pDumper.writeFieldValue("Text color", element.getTextColor());
		pDumper.writeFieldValue("Text weight", element.getTextWeight());
		pDumper.writeFieldValue("Text font entry id", element.getTextFontId());
		pDumper.writeFieldValue("Dot symbol font entry id", element.getDotSymbolFontId());
		pDumper.writeFieldValue("Dot symbol code", element.getDotSymbolCode());
		pDumper.dumpFieldName("Extended style flags:");
		pDumper.writeFieldValue("  Angle leading zero flag", element.getAngleLeadingZeroFlag());
		pDumper.writeFieldValue("  Angle trailing zeros flag", element.getAngleTrailingZerosFlag());
		pDumper.writeFieldValue("  Auto superscript char flag", element.getAutoSuperscriptCharFlag());
		pDumper.writeFieldValue("Dimension line style entry id", element.getDimensionLineStyleId());
		pDumper.writeFieldValue("Lower text margin", element.getTextLowerMargin());
		pDumper.writeFieldValue("Left text margin", element.getTextLeftMargin());
		pDumper.writeFieldValue("Left tolerance margin", element.getToleranceLeftMargin());
		pDumper.writeFieldValue("Separator tolerance margin", element.getToleranceSepMargin());
		pDumper.writeFieldValue("Terminator height", element.getTerminatorHeight());
		pDumper.writeFieldValue("Terminator width", element.getTerminatorWidth());
		pDumper.writeFieldValue("Text width", element.getTextWidth());
		pDumper.dumpFieldName("Place flags:");
		pDumper.writeFieldValue("  Text width override", element.getTextWidthOverrideFlag());
		pDumper.writeFieldValue("  Extension line style override", element.getExtensionLineStyleOverrideFlag());
		pDumper.writeFieldValue("  Extension line weight override", element.getExtensionLineWeightOverrideFlag());
		pDumper.writeFieldValue("  Extension line color override", element.getExtensionLineColorOverrideFlag());
		pDumper.writeFieldValue("  Terminator style override", element.getTerminatorLineStyleOverrideFlag());
		pDumper.writeFieldValue("  Terminator weight override", element.getTerminatorLineWeightOverrideFlag());
		pDumper.writeFieldValue("  Terminator color override", element.getTerminatorColorOverrideFlag());
		pDumper.writeFieldValue("  Note frame type", element.getNoteFrameType());
		pDumper.writeFieldValue("  Use note inline leader", element.getNoteInlineLeaderFlag());
		pDumper.writeFieldValue("  Note justification", element.getNoteJustification());
		pDumper.writeFieldValue("  Use thousand separator", element.getMetricSpaceFlag());
		pDumper.writeFieldValue("  Comma thousand separator", element.getThousandSeparatorFlag());
		pDumper.writeFieldValue("  Stacked fraction type", element.getStackedFractionType());
		pDumper.writeFieldValue("  Stacked fraction alignment", element.getStackedFractionAlign());
		pDumper.writeFieldValue("  Use Stacked fraction", element.getUseStackedFractionFlag());
		pDumper.writeFieldValue("  Uniform cell scale", element.getUniformCellScaleFlag());
		pDumper.writeFieldValue("Extension line style id", element.getExtensionLineStyleId());
		pDumper.writeFieldValue("Extension line weight", element.getExtensionLineWeight());
		pDumper.writeFieldValue("Extension line color", element.getExtensionLineColor());
		pDumper.writeFieldValue("Terminator line style id", element.getTerminatorLineStyleId());
		pDumper.writeFieldValue("Terminator line weight", element.getTerminatorLineWeight());
		pDumper.writeFieldValue("Terminator color", element.getTerminatorColor());
		pDumper.dumpFieldName("Draw flags:");
		pDumper.writeFieldValue("  Arrow symbol type", element.getArrowSymbolType());
		pDumper.writeFieldValue("  Stroke symbol type", element.getStrokeSymbolType());
		pDumper.writeFieldValue("  Origin symbol type", element.getOriginSymbolType());
		pDumper.writeFieldValue("  Dot symbol type", element.getDotSymbolType());
		pDumper.writeFieldValue("  Prefix symbol type", element.getPrefixSymbolType());
		pDumper.writeFieldValue("  Suffix symbol type", element.getSuffixSymbolType());
		pDumper.writeFieldValue("  Diameter symbol type", element.getDiameterSymbolType());
		pDumper.writeFieldValue("  Plus/minus symbol type", element.getPlusMinusSymbolType());
		pDumper.writeFieldValue("  No line through arrow terminator", element.getNoLineThroughArrowTerminatorFlag());
		pDumper.writeFieldValue("  No line through stroke terminator", element.getNoLineThroughStrokeTerminatorFlag());
		pDumper.writeFieldValue("  No line through origin terminator", element.getNoLineThroughOriginTerminatorFlag());
		pDumper.writeFieldValue("  No line through dot terminator", element.getNoLineThroughDotTerminatorFlag());
		pDumper.writeFieldValue("  Underline override", element.getUnderlineOverrideFlag());
		pDumper.writeFieldValue("Prefix font id", element.getPrefixFont());
		pDumper.writeFieldValue("Suffix font id", element.getSuffixFont());
		pDumper.writeFieldValue("Prefix symbol code", element.getPrefixChar());
		pDumper.writeFieldValue("Suffix symbol code", element.getSuffixChar());
		pDumper.writeFieldValue("Plus/Minus symbol code", element.getPlusMinusChar());
		pDumper.dumpFieldName("Primary Alt format:");
		pDumper.writeFieldValue("  Threshold", element.getPrimaryAltThreshold());
		pDumper.writeFieldValue("  Label display mode", element.getPrimaryAltLabelDisplayMode());
		pDumper.writeFieldValue("  Comparison operator", element.getPrimaryAltOperator());
		pDumper.writeFieldValue("  Subunit threshold", element.getPrimaryAltSubunitThresholdFlag());
		pDumper.writeFieldValue("  Show zero master units", element.getPrimaryAltShowZeroMasterUnitsFlag());
		pDumper.writeFieldValue("  Show zero sub units", !element.getPrimaryAltHideZeroSubUnitsFlag());
		pDumper.writeFieldValue("  Enable alt format", element.getPrimaryAltPresentFlag());
		pDumper.writeFieldValue("  Accuracy", element.getPrimaryAltAccuracy());
		pDumper.dumpFieldName("Ball and chain Flags:");
		pDumper.writeFieldValue("  Leader terminator", element.getDimLeaderTerminator());
		pDumper.writeFieldValue("  Leader chain type", element.getDimLeaderChainType());
		pDumper.writeFieldValue("  Leader text alignment", element.getDimLeaderAlignment());
		pDumper.writeFieldValue("  Inline leader flag", element.getDimLeaderInlineLeaderFlag());
		pDumper.writeFieldValue("  Leader enable flag", element.getDimLeaderEnableFlag());
		pDumper.writeFieldValue("  Leader undock text flag", element.getDimLeaderUndockTextFlag());
		pDumper.dumpFieldName("Secondary Alt format:");
		pDumper.writeFieldValue("  Threshold", element.getSecondaryAltThreshold());
		pDumper.writeFieldValue("  Label display mode", element.getSecondaryAltLabelDisplayMode());
		pDumper.writeFieldValue("  Comparison operator", element.getSecondaryAltOperator());
		pDumper.writeFieldValue("  Subunit threshold", element.getSecondaryAltSubunitThresholdFlag());
		pDumper.writeFieldValue("  Show zero master units", element.getSecondaryAltShowZeroMasterUnitsFlag());
		pDumper.writeFieldValue("  Show zero sub units", !element.getSecondaryAltHideZeroSubUnitsFlag());
		pDumper.writeFieldValue("  Enable alt format", element.getSecondaryAltPresentFlag());
		pDumper.writeFieldValue("  Accuracy", element.getSecondaryAltAccuracy());
		//  pDumper.writeFieldValue( "Primary master units", element.getPrimaryMasterUnits());
		//  pDumper.writeFieldValue( "Primary sub units", element.getPrimarySubUnits());
		//  pDumper.writeFieldValue( "Secondary master units", element.getSecondaryMasterUnits());
		//  pDumper.writeFieldValue( "Secondary sub units", element.getSecondarySubUnits());
		pDumper.writeFieldValue("Annotation scale", element.getAnnotationScale());
		pDumper.writeFieldValue("Primary tolerance accuracy", element.getPrimaryToleranceAccuracy());
		pDumper.writeFieldValue("Secondary tolerance accuracy", element.getSecondaryToleranceAccuracy());
		pDumper.writeFieldValue("Stacked fraction scale", element.getStackedFractionScale());
		pDumper.writeFieldValue("Datum value", element.getOrdinateDimensionDatumValue());
		pDumper.writeFieldValue("Dimension leader inline leader length", element.getDimLeaderInlineLeaderLength());
		pDumper.writeFieldValue("Decrement in reverse direction flag", element.getOrdinateDimDecrementInReverseDirectionFlag());
		pDumper.writeFieldValue("Ordinate dimension free text location flag", element.getOrdinateDimFreeLocationTextFlag());
		pDumper.writeFieldValue("Enable term. min leader length flag", element.getEnableTerminatorMinLeaderLengthFlag());
		pDumper.writeFieldValue("Enable suppress unfit term. flag", element.getEnableSuppressUnfitTerminatorFlag());
		pDumper.writeFieldValue("Enable dimension inline leader flag", element.getDimLeaderEnableInlineLeaderLengthFlag());
		pDumper.writeFieldValue("Enable text above optimal fit flag", element.getEnableTextAboveOptimalFitFlag());
		pDumper.writeFieldValue("Fit true dimension text is Narrow", element.getEnableNarrowFontOptimalFitFlag());
		pDumper.writeFieldValue("Primary retain fractional accuracy flag", element.getPrimaryRetainFractionalAccuracyFlag());
		pDumper.writeFieldValue("Secondary retain fractional accuracy flag", element.getSecondaryRetainFractionalAccuracyFlag());
		pDumper.writeFieldValue("Primary tolerance retain fractional accuracy flag", element.getPrimaryToleranceRetainFractionalAccuracyFlag());
		pDumper.writeFieldValue("Secondary tolerance retain fractional accuracy flag", element.getSecondaryToleranceRetainFractionalAccuracyFlag());
		pDumper.writeFieldValue("Primary alt. format retain fractional accuracy flag", element.getPrimaryAltRetainFractionalAccuracyFlag());
		pDumper.writeFieldValue("Secondary alt. format retain fractional accuracy flag", element.getSecondaryAltRetainFractionalAccuracyFlag());
		pDumper.writeFieldValue("Fit options", element.getFitOptions());
		pDumper.writeFieldValue("Label-line mode", element.getLabelLineDimensionMode());
		pDumper.writeFieldValue("Note spline fit flag", element.getNoteSplineFitFlag());
	}
}
class OdDgMultilineStyleTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgMultilineStyleTableRecord element = (OdDgMultilineStyleTableRecord)pObj;

		pDumper.writeFieldValue("Uses fill color", element.getUseFillColorFlag());
		pDumper.writeFieldValue("Fill color", element.getFillColorIndex());
		pDumper.writeFieldValue("Origin cap angle", element.getOriginCapAngle());
		pDumper.writeFieldValue("End cap angle", element.getEndCapAngle());
		{
			OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

			element.getOriginCap(symbology);
			pDumper.writeFieldValue("Origin cap", symbology);
			element.getMiddleCap(symbology);
			pDumper.writeFieldValue("End cap", symbology);
			element.getEndCap(symbology);
			pDumper.writeFieldValue("Middle cap", symbology);
		}

		{
			uint j = element.getProfilesCount();
			pDumper.writeFieldValue("Number of profiles", j);
			for (uint i = 0; i < j; i++)
			{
				OdDgMultilineProfile profile = new OdDgMultilineProfile();
				element.getProfile(i, profile);
				pDumper.writeFieldValue("Profile " + i.ToString(), profile);
			}
		}
	}
}
class OdDgMaterialTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
	}
}

class OdDgLineStyleTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLineStyleTableRecord pRec = (OdDgLineStyleTableRecord)pObj;

		pDumper.writeFieldValueHex("EntryId", pRec.getEntryId());
		pDumper.writeFieldValue("RefersToElement Flag", pRec.getRefersToElementFlag());
		pDumper.writeFieldValue("Snappable Flag", pRec.getSnappableFlag());
		pDumper.writeFieldValue("Units Type", pRec.getUnitsType());
		pDumper.writeFieldValue("NoSnap Flag", pRec.getNoSnapFlag());
		pDumper.writeFieldValue("Continuous Flag", pRec.getContinuousFlag());
		pDumper.writeFieldValue("NoRange Flag", pRec.getNoRangeFlag());
		pDumper.writeFieldValue("SharedCellScaleIndependent Flag", pRec.getSharedCellScaleIndependentFlag());
		pDumper.writeFieldValue("NoWidth Flag", pRec.getNoWidthFlag());
		pDumper.writeFieldValue("RefersToId", pRec.getRefersToId().getHandle().ascii());
		pDumper.writeFieldValue("Type", pRec.getType());
		pDumper.writeFieldValue("RscFileName", pRec.getRscFileName());
	}
}
class OdDgLineStyleDefTableRecordDumperPE : OdDgTableRecordDumperPE
{
	public override String getName(OdRxObject pObj)
	{
		OdDgTableRecord pRec = (OdDgTableRecord)pObj;
		string s = pRec.getName();
		if (s == "")
			s = "<Empty>";
		return s;
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLineStyleDefTableRecord pRec = (OdDgLineStyleDefTableRecord)pObj;

		LineStyleType type = pRec.getType();
		pDumper.writeFieldValue("Type", type);

		OdDgLineStyleResource pRes = pRec.getResource();
		OdDgRxObjectDumperPE pResDumper = OdDgRxObjectDumperPE.getDumper(pRes.isA());
		if (pResDumper != null)
		{
			pResDumper.dump(pRes, pDumper);
		}
	}
}
class OdDgDictionaryTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgDictionaryTableRecord pRec = (OdDgDictionaryTableRecord)pObj;
		pDumper.writeFieldValue("ItemId", pRec.getItemId());
	}
}

class OdDgRegAppTableRecordDumperPE : OdDgTableRecordDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		//OdDgRegAppTableRecord element = pObj;
	}
}

class OdDgViewGroupDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgViewGroup pComplElm = (OdDgViewGroup)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgViewGroup element = (OdDgViewGroup)pObj;
		pDumper.writeFieldValue("Name", element.getName());
		pDumper.writeFieldValue("ModelId", element.getModelId());
		pDumper.writeFieldValue("Active Level", element.getActiveLevelEntryId());
		pDumper.writeFieldValueHex("Active Color", element.getActiveColor());
		pDumper.writeFieldValueHex("Active Line Style Id", element.getActiveLineStyleId());
		pDumper.writeFieldValueHex("Active Line Weight", element.getActiveLineWeight());
		pDumper.writeFieldValue("Dwg Display Order", element.getDwgDisplayOrder());
	}
}
class OdDgViewDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgView pComplElm = (OdDgView)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgView element = (OdDgView)pObj;

		pDumper.writeFieldValue("Model ID", element.getModelId());

		pDumper.writeFieldValue("Is named", element.isNamed());
		pDumper.writeFieldValue("Name", element.getName());

		pDumper.writeFieldValue("Index", element.getIndex());

		pDumper.writeFieldValue("View rectangle", element.getViewRectangle());

		pDumper.writeFieldValue("Is visible", element.getVisibleFlag());
		pDumper.writeFieldValue("Fast curves", element.getFastCurveFlag());
		pDumper.writeFieldValue("Hide texts", element.getHideTextsFlag());
		pDumper.writeFieldValue("High quality fonts", element.getHighQualityFontsFlag());
		pDumper.writeFieldValue("Show line weights", element.getShowLineWeightsFlag());
		pDumper.writeFieldValue("Show patterns", element.getShowPatternsFlag());
		pDumper.writeFieldValue("Show text nodes", element.getShowTextNodesFlag());
		pDumper.writeFieldValue("Show data fields", element.getShowDataFieldsFlag());
		pDumper.writeFieldValue("Show grid", element.getShowGridFlag());
		pDumper.writeFieldValue("Show level symbology", element.getShowLevelSymbologyFlag());
		pDumper.writeFieldValue("Show points", element.getShowPointsFlag());
		pDumper.writeFieldValue("Show construction", element.getShowConstructionFlag());
		pDumper.writeFieldValue("Show dimensions", element.getShowDimensionsFlag());
		pDumper.writeFieldValue("Fast cells", element.getFastCellsFlag());
		pDumper.writeFieldValue("Is defined", element.getDefinedFlag());
		pDumper.writeFieldValue("Show fills", element.getShowFillsFlag());
		pDumper.writeFieldValue("Show raster text", element.getShowRasterTextFlag());
		pDumper.writeFieldValue("Show axis triad", element.getShowAxisTriadFlag());
		pDumper.writeFieldValue("Orientation display", element.getOrientationDisplayFlag());
		pDumper.writeFieldValue("View rendered", element.getViewRenderedFlag());
		pDumper.writeFieldValue("Show background", element.getShowBackgroundFlag());
		pDumper.writeFieldValue("Show boundary", element.getShowBoundaryFlag());
		pDumper.writeFieldValue("Fast boundary clip", element.getFastBoundaryClipFlag());
		pDumper.writeFieldValue("Use perspective camera", element.getUseCameraFlag());
		pDumper.writeFieldValue("Use depth cueing", element.getUseDepthCueFlag());
		pDumper.writeFieldValue("Inhibit dynamics", element.getInhibitDynamicsFlag());
		pDumper.writeFieldValue("Show shadows", element.getShowShadowsFlag());
		pDumper.writeFieldValue("Show texture maps", element.getShowTextureMapsFlag());
		pDumper.writeFieldValue("Show haze", element.getShowHazeFlag());
		pDumper.writeFieldValue("Use transparency", element.getUseTransparencyFlag());
		pDumper.writeFieldValue("Ignore line styles", element.getIgnoreLineStylesFlag());
		pDumper.writeFieldValue("Is accelerated", element.getAcceleratedFlag());
		pDumper.writeFieldValue("Is pattern dynamics", element.getPatternDynamicsFlag());
		pDumper.writeFieldValue("Hidden lines", element.getHiddenLineFlag());
		pDumper.writeFieldValue("Show tags", element.getShowTagsFlag());
		pDumper.writeFieldValue("Display edges", element.getDisplayEdgesFlag());
		pDumper.writeFieldValue("Display hidden edges", element.getDisplayHiddenEdgesFlag());
		pDumper.writeFieldValue("Is named", element.getNamedFlag());
		pDumper.writeFieldValue("Override background", element.getOverrideBackgroundFlag());
		pDumper.writeFieldValue("Show front clip", element.getShowClipFrontFlag());
		pDumper.writeFieldValue("Show back clip", element.getShowClipBackFlag());
		pDumper.writeFieldValue("Show volume clip", element.getShowClipVolumeFlag());
		pDumper.writeFieldValue("Use display set", element.getUseDisplaySetFlag());

		pDumper.writeFieldValue("Display Mode", element.getDisplayMode());

		if (element.getUseCameraFlag())
		{
			OdGeMatrix3d rotation = new OdGeMatrix3d();
			OdGePoint3d position = new OdGePoint3d();
			OdGeExtents2d rectangle = new OdGeExtents2d();
			element.getCameraRotation(rotation);
			element.getCameraPosition(position);
			element.getCameraVisibleRectangle(rectangle);

			pDumper.writeFieldValue("Position", position);
			pDumper.writeFieldValue("Rotation", rotation);
			pDumper.writeFieldValue("Focal length", element.getCameraFocalLength());
			pDumper.writeFieldValue("Visible rectangle", rectangle);
			pDumper.writeFieldValue("Front clipping", element.getCameraFrontClippingDistance());
			pDumper.writeFieldValue("Back clipping", element.getCameraBackClippingDistance());
		}
		else
		{
			OdGeMatrix3d rotation = new OdGeMatrix3d();
			OdGeExtents3d box = new OdGeExtents3d();
			element.getOrthographyRotation(rotation);
			element.getOrthographyVisibleBox(box);

			pDumper.writeFieldValue("Rotation", rotation);
			pDumper.writeFieldValue("Visible box", box);
		}
	}
}
class OdDgLevelMaskDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLevelMask pLevelMask = (OdDgLevelMask)pObj;
		UInt32 maxLevelEntryId = pLevelMask.getMaxLevelEntryId();
		pDumper.writeFieldValue("MaxLevelEntryId", maxLevelEntryId);
		pDumper.writeFieldValue("View Number", pLevelMask.getViewIndex());

		OdDgLevelTable pTable = null;

		if (pLevelMask.getReferenceAttachId().isNull())
		{
			pDumper.writeFieldValueHex("Reference Id", (UInt64)(0));

			bool bXRefMask = false;

			OdDgElementId idOwner = pLevelMask.ownerId();

			if (idOwner != null)
			{
				OdDgElement pOwner = (OdDgElement)idOwner.openObject(OpenMode.kForRead);

				if (pOwner.getElementType() == OdDgElement.ElementTypes.kTypeReferenceAttachmentHeader)
				{
					OdDgReferenceAttachmentHeader pXRef = (OdDgReferenceAttachmentHeader)pOwner;
					pTable = pXRef.getLevelTable(OpenMode.kForRead);
					bXRefMask = true;
				}
			}

			if (!bXRefMask)
			{
				pTable = pLevelMask.database().getLevelTable();
			}
		}
		else
		{
			pDumper.writeFieldValueHex("Reference Id", (UInt64)(pLevelMask.getReferenceAttachId().getHandle()));
			OdDgReferenceAttachmentHeader pXRef = (OdDgReferenceAttachmentHeader)pLevelMask.getReferenceAttachId().openObject();
			pTable = pXRef.getLevelTable();
		}

		if (pTable != null)
		{
			OdDgElementIterator pIt = pTable.createIterator();
			for (; !pIt.done(); pIt.step())
			{
				OdDgLevelTableRecord pLevel = (OdDgLevelTableRecord)pIt.item().safeOpenObject();
				String levelName = pLevel.getName();
				UInt32 levelEntryId = pLevel.getEntryId();
				bool levelIsVisible = true;
				if (levelEntryId <= maxLevelEntryId || levelEntryId == 64)
				{
					levelIsVisible = pLevelMask.getLevelIsVisible(levelEntryId);
				}
				pDumper.writeFieldValue(levelName, levelIsVisible);
			}
		}
		else
		{
			pDumper.writeFieldValue("Can't load level table", "!!!");
		}
	}
}
//----------------------------------------------------------
//
// Elements
//
//----------------------------------------------------------
class OdDgSharedCellDefinitionDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgSharedCellDefinition pComplElm = (OdDgSharedCellDefinition)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgSharedCellDefinition element = (OdDgSharedCellDefinition)pObj;

		pDumper.writeFieldValue("Name", element.getName());
		pDumper.writeFieldValue("Description", element.getDescription());

		pDumper.writeFieldValue("Origin", element.getOrigin());
	}
	public override string getName(OdRxObject pObj)
	{
		OdDgSharedCellDefinition pElm = (OdDgSharedCellDefinition)pObj;
		return pElm.getName();
	}
}
class OdDgSharedCellReferenceDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgSharedCellReference element = (OdDgSharedCellReference)pObj;

		pDumper.writeFieldValue("Name of definition", element.getDefinitionName());

		pDumper.writeFieldValue("Transformation", element.getTransformation());
		pDumper.writeFieldValue("Origin", element.getOrigin());

		pDumper.writeFieldValueHex("Class map", element.getClassMap());

		pDumper.writeFieldValue("Override level", element.getLevelOverrideFlag());
		pDumper.writeFieldValue("Override relative", element.getRelativeOverrideFlag());
		pDumper.writeFieldValue("Override class", element.getClassOverrideFlag());
		pDumper.writeFieldValue("Override color", element.getColorOverrideFlag());
		pDumper.writeFieldValue("Override weight", element.getWeightOverrideFlag());
		pDumper.writeFieldValue("Override style", element.getStyleOverrideFlag());
		pDumper.writeFieldValue("Override associative point", element.getAssociativePointOverrideFlag());
	}
}


class OdDgTagDefinitionSetDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgTagDefinitionSet pTagDefinitionsSet = (OdDgTagDefinitionSet)pObj;

		pDumper.writeFieldValue("Name", pTagDefinitionsSet.getName());

		UInt32 j = pTagDefinitionsSet.getCount();
		pDumper.writeFieldValue("Number of definitions", j);

		//OdRxObject                   pObj;
		//OdSmartPtr< OdDgObject_Dumper > dumper;

		//for( i = 0; i < j; i++ )
		//{
		//  pObj = pTagDefinitionsSet.getByIndex( i );
		//  dumper = pObj;
		//  dumper.dumpData( pObj );
		//}
	}
}
class OdDgTagDefinitionDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		OdDgTagDefinition element = (OdDgTagDefinition)pObj;

		pDumper.writeFieldValue("Type", element.getType());
		pDumper.writeFieldValue("ID", element.getEntryId());
		pDumper.writeFieldValue("Name", element.getName());
		pDumper.writeFieldValue("Prompt", element.getPrompt());
		switch (element.getType())
		{
			case OdDgTagDefinition.Type.kChar:
				pDumper.writeFieldValue("Default char value", element.getString());
				break;
			case OdDgTagDefinition.Type.kInt16:
				pDumper.writeFieldValue("Default int16 value", element.getInt16());
				break;
			case OdDgTagDefinition.Type.kInt32:
				pDumper.writeFieldValue("Default int32 value", element.getInt32());
				break;
			case OdDgTagDefinition.Type.kDouble:
				pDumper.writeFieldValue("Default double value", element.getDouble());
				break;
			case OdDgTagDefinition.Type.kBinary:
				pDumper.writeFieldValue("Default binary data (size)", element.getBinarySize());
				break;
			default:
				Debug.Fail("Invalid element type");
				break;
		}
	}
}

class OdDgColorTableDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgColorTable element = (OdDgColorTable)pObj;
		UInt32[] p = element.Palette;
		for (int i = 0; i < p.Length; i++)
		{
			pDumper.writeFieldValueHex("Color " + i.ToString(), p[i]);
		}
	}
}

class OdDgElementTemplateTableDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgElementTemplate element = (OdDgElementTemplate)pObj;

		pDumper.writeFieldValue("  Template Name", element.getTemplateName());
		pDumper.writeFieldValue("  Order In Group", element.getTemplateOrder());
		pDumper.writeFieldValueHex("  Parent Id", element.getParentId());
		pDumper.writeFieldValue("  Template Group Flag", element.isTemplateGroup());

		for (UInt32 i = 0; i < element.getItemCount(); i++)
		{
			OdDgTemplateItem tmpItem = element.getTemplateItem(i);

			String strPathName;

			strPathName = String.Format("  Template item {0} :", i);

			pDumper.dumpFieldName(strPathName);

			strPathName = "";

			for (UInt32 k = 0; k < tmpItem.getPathLength(); k++)
			{
				strPathName += tmpItem.getPathItem(k);

				if (k != tmpItem.getPathLength() - 1)
				{
					strPathName += ".";
				}
			}

			pDumper.writeFieldValue("     Path:", strPathName);

			for (UInt32 n = 0; n < tmpItem.getValueCount(); n++)
			{
				String strValueName;

				strValueName = String.Format("     Value {0}", n + 1);

				OdDgTemplateValueVariant value = tmpItem.getValue(n);

				pDumper.writeFieldValue(strValueName, value);
			}
		}

	}
}

class OdDgApplicationDataDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgApplicationData pElm = (OdDgApplicationData)pObj;

		OdBinaryData data = new OdBinaryData();
		pElm.getData(data);
		pDumper.writeFieldValueHex("Signature", pElm.getSignature());
		pDumper.writeFieldValue("Binary data size", data.Count);
	}
}

class OdDgReferenceAttachmentHeaderDumperPE : OdDgElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgReferenceAttachmentHeader pComplElm = (OdDgReferenceAttachmentHeader)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgReferenceAttachmentHeader element = (OdDgReferenceAttachmentHeader)pObj;

		pDumper.writeFieldValue("The full reference path", element.getFullFileName());
		pDumper.writeFieldValue("The base file name", element.getFileName());
		pDumper.writeFieldValue("ModelName", element.getModelName());
		pDumper.writeFieldValue("LogicalName", element.getLogicalName());
		pDumper.writeFieldValue("MasterOrigin", element.getMasterOrigin());
		pDumper.writeFieldValue("ReferenceOrigin", element.getReferenceOrigin());
		pDumper.writeFieldValue("Transformation", element.getTransformation());

		pDumper.writeFieldValue("Description", element.getDescription());
		pDumper.writeFieldValue("FileNumber", element.getFileNumber());
		pDumper.writeFieldValue("Priority", element.getPriority());
		pDumper.writeFieldValue("BaseNestDepth", element.getBaseNestDepth());
		pDumper.writeFieldValue("NestDepth", element.getNestDepth());
		pDumper.writeFieldValue("Scale", element.getScale());
		pDumper.writeFieldValue("Entire Scale", element.getEntireScale());
		pDumper.writeFieldValue("ZFront", element.getZFront());
		pDumper.writeFieldValue("ZBack", element.getZBack());
		pDumper.writeFieldValue("CameraPosition", element.getCameraPosition());
		pDumper.writeFieldValue("CameraFocalLength", element.getCameraFocalLength());

		pDumper.writeFieldValue("ClipPointsCount", element.getClipPointsCount());
		for (UInt32 i = 0, nCount = element.getClipPointsCount(); i < nCount; i++)
		{
			pDumper.writeFieldValue("ClipPoint " + i.ToString(), element.getClipPoint(i));
		}

		pDumper.writeFieldValue("CoincidentFlag", element.getCoincidentFlag());
		pDumper.writeFieldValue("SnapLockFlag", element.getSnapLockFlag());
		pDumper.writeFieldValue("LocateLockFlag", element.getLocateLockFlag());
		pDumper.writeFieldValue("CompletePathInV7Flag", element.getCompletePathInV7Flag());
		pDumper.writeFieldValue("AnonymousFlag", element.getAnonymousFlag());
		pDumper.writeFieldValue("InactiveFlag", element.getInactiveFlag());
		pDumper.writeFieldValue("MissingFileFlag", element.getMissingFileFlag());
		pDumper.writeFieldValue("LevelOverride", element.getLevelOverride());
		pDumper.writeFieldValue("DontDetachOnAllFlag", element.getDontDetachOnAllFlag());
		pDumper.writeFieldValue("MetadataOnlyFlag", element.getMetadataOnlyFlag());
		pDumper.writeFieldValue("DisplayFlag", element.getDisplayFlag());
		pDumper.writeFieldValue("LineStyleScaleFlag", element.getLineStyleScaleFlag());
		pDumper.writeFieldValue("HiddenLineFlag", element.getHiddenLineFlag());
		pDumper.writeFieldValue("DisplayHiddenLinesFlag", element.getDisplayHiddenLinesFlag());
		pDumper.writeFieldValue("RotateClippingFlag", element.getRotateClippingFlag());
		pDumper.writeFieldValue("ExtendedRefFlag", element.getExtendedRefFlag());
		pDumper.writeFieldValue("ClipBackFlag", element.getClipBackFlag());
		pDumper.writeFieldValue("ClipFrontFlag", element.getClipFrontFlag());
		pDumper.writeFieldValue("CameraOnFlag", element.getCameraOnFlag());
		pDumper.writeFieldValue("TrueScaleFlag", element.getTrueScaleFlag());
		pDumper.writeFieldValue("DisplayBoundaryFlag", element.getDisplayBoundaryFlag());
		pDumper.writeFieldValue("LibraryRefFlag", element.getLibraryRefFlag());
		pDumper.writeFieldValue("DisplayRasterRefsFlag", element.getDisplayRasterRefsFlag());
		pDumper.writeFieldValue("UseAlternateFileFlag", element.getUseAlternateFileFlag());
		pDumper.writeFieldValue("UseLightsFlag", element.getUseLightsFlag());
		pDumper.writeFieldValue("DoNotDisplayAsNestedFlag", element.getDoNotDisplayAsNestedFlag());
		pDumper.writeFieldValue("ColorTableUsage", element.getColorTableUsage());
		pDumper.writeFieldValue("ViewportFlag", element.getViewportFlag());
		pDumper.writeFieldValue("ScaleByStorageUnitsFlag", element.getScaleByStorageUnitsFlag());
		pDumper.writeFieldValue("PrintColorAdjustmentFlag", element.getPrintColorAdjustmentFlag());
	}
}


class OdDgGraphicsElementDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgGraphicsElement pElm = (OdDgGraphicsElement)pObj;

		pDumper.writeFieldValue("Level ID", pElm.getLevelEntryId());
		pDumper.writeFieldValue("Color index", pElm.getColorIndex());
		pDumper.writeFieldValue("Graphics group", pElm.getGraphicsGroupEntryId());
		pDumper.writeFieldValue("Class", pElm.getClass());
		pDumper.writeFieldValueHex("Line style", (UInt32)pElm.getLineStyleEntryId());
		pDumper.writeFieldValue("Line weight", pElm.getLineWeight());
		pDumper.writeFieldValue("Thickness", pElm.getThickness());
		pDumper.writeFieldValue("Priority", pElm.getPriority());
		pDumper.writeFieldValue("Is Invisible", pElm.getInvisibleFlag());
		pDumper.writeFieldValue("Is 3D Format Element", pElm.get3dFormatFlag());
		pDumper.writeFieldValue("View Independent", pElm.getViewIndependentFlag());
		pDumper.writeFieldValue("Non Planar", pElm.getNonPlanarFlag());
		pDumper.writeFieldValue("Not Snappable", pElm.getNotSnappableFlag());
		pDumper.writeFieldValue("Hbit", pElm.getHbitFlag());

		// Extents
		{
			OdGeExtents3d extent = new OdGeExtents3d();
			if (pElm.getGeomExtents(extent) == OdResult.eOk)
			{
				pDumper.writeFieldValue("Extent", extent);
			}
			else
			{
				pDumper.writeFieldValue("Extent", "Invalid value");
			}
		}
	}
}
class OdDgLine2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLine2d pElm = (OdDgLine2d)pObj;
		pDumper.writeFieldValue("Vertex 1", pElm.getStartPoint());
		pDumper.writeFieldValue("Vertex 2", pElm.getEndPoint());
	}
}
class OdDgLine3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLine3d pElm = (OdDgLine3d)pObj;
		pDumper.writeFieldValue("Vertex 1", pElm.getStartPoint());
		pDumper.writeFieldValue("Vertex 2", pElm.getEndPoint());
	}
}


class OdDgLineString2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLineString2d element = (OdDgLineString2d)pObj;
		uint j = element.getVerticesCount();

		pDumper.writeFieldValue("Number of vertices", j);
		for (uint i = 0; i < j; i++)
		{
			OdGePoint2d point = element.getVertexAt(i);
			pDumper.writeFieldValue("Vertex " + i.ToString(), point);
		}
	}
}


class OdDgLineString3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLineString3d element = (OdDgLineString3d)pObj;
		uint j = element.getVerticesCount();
		pDumper.writeFieldValue("Number of vertices", j);
		for (uint i = 0; i < j; i++)
		{
			OdGePoint3d point = element.getVertexAt(i);
			pDumper.writeFieldValue("Vertex " + i.ToString(), point);
		}
	}
}



class OdDgText2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgText2d element = (OdDgText2d)pObj;
		pDumper.writeFieldValue("Text", element.getText());
		pDumper.writeFieldValue("Font ID", element.getFontEntryId());
		// Gets Font name
		OdDgFontTable pFontTable = element.database().getFontTable();
		OdDgFontTableRecord pFont = pFontTable.getFont(element.getFontEntryId());
		if (pFont != null)
		{
			pDumper.writeFieldValue("Font Name", pFont.getName());
		}

		pDumper.writeFieldValue("Justification", element.getJustification());
		pDumper.writeFieldValue("Length multiplier", element.getLengthMultiplier());
		pDumper.writeFieldValue("Height multiplier", element.getHeightMultiplier());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Origin", element.getOrigin());

		pDumper.writeFieldValue("TextStyle bit", element.getTextStyleFlag());
		pDumper.writeFieldValue("TextStyle ID", element.getTextStyleEntryId());

		OdDgTextStyleTable pTextStyleTable = element.database().getTextStyleTable();
		OdDgElementId id = pTextStyleTable.getAt(element.getTextStyleEntryId());
		if (!id.isNull())
		{
			OdDgTextStyleTableRecord pTextStyle = (OdDgTextStyleTableRecord)id.safeOpenObject();
			pDumper.writeFieldValue("TextStyle Name", pTextStyle.getName());
		}

		Int16 nCount = element.getTextEditFieldCount();
		pDumper.writeFieldValue("The number of enter data fields in the text element is ", nCount);

		for (Int16 i = 0; i < nCount; i++)
		{
			OdDgTextEditField textEditField = element.getTextEditFieldAt(i);

			pDumper.writeFieldValue("StartPosition", element.getHeight());
			pDumper.writeFieldValue("Length", element.getRotation());
			pDumper.writeFieldValue("Justification", element.getOrigin());
		}
	}
}



class OdDgText3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgText3d element = (OdDgText3d)pObj;
		pDumper.writeFieldValue("Text", element.getText());
		pDumper.writeFieldValue("Font ID", element.getFontEntryId());
		// Gets Font name
		OdDgFontTable pFontTable = element.database().getFontTable();
		OdDgFontTableRecord pFont = pFontTable.getFont(element.getFontEntryId());
		if (pFont != null)
		{
			pDumper.writeFieldValue("Font Name", pFont.getName());
		}
		pDumper.writeFieldValue("Justification", element.getJustification());
		pDumper.writeFieldValue("Length multiplier", element.getLengthMultiplier());
		pDumper.writeFieldValue("Height multiplier", element.getHeightMultiplier());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Origin", element.getOrigin());

		//Fields
		{
			Int16 j = element.getTextEditFieldCount();
			pDumper.writeFieldValue("Number of EDFields: ", j);
			for (Int16 i = 0; i < j; i++)
			{
				OdDgTextEditField value = element.getTextEditFieldAt(i);
				pDumper.writeFieldValue("EDField " + i.ToString(), value);
			}
		}
	}
}


class OdDgTextNode2dDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgTextNode2d pComplElm = (OdDgTextNode2d)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgTextNode2d element = (OdDgTextNode2d)pObj;

		pDumper.writeFieldValue("Line spacing", element.getLineSpacing());
		pDumper.writeFieldValue("Font ID", element.getFontEntryId());
		pDumper.writeFieldValue("Max length", element.getMaxLength());
		pDumper.writeFieldValue("Justification", element.getJustification());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Origin", element.getOrigin());
	}
}


class OdDgTextNode3dDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgTextNode3d pComplElm = (OdDgTextNode3d)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgTextNode3d element = (OdDgTextNode3d)pObj;
		pDumper.writeFieldValue("Line spacing", element.getLineSpacing());
		pDumper.writeFieldValue("Font ID", element.getFontEntryId());
		pDumper.writeFieldValue("Max length", element.getMaxLength());
		pDumper.writeFieldValue("Justification", element.getJustification());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Origin", element.getOrigin());
	}
}


class OdDgShape2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgShape2d element = (OdDgShape2d)pObj;
		OdGePoint2d point;
		uint j = element.getVerticesCount();

		pDumper.writeFieldValue("Number of vertices", j);
		for (uint i = 0; i < j; i++)
		{
			point = element.getVertexAt(i);
			pDumper.writeFieldValue("Vertex " + i, point);
		}
	}
}


class OdDgShape3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgShape3d element = (OdDgShape3d)pObj;
		uint j = element.getVerticesCount();
		pDumper.writeFieldValue("Number of vertices", j);
		for (uint i = 0; i < j; i++)
		{
			OdGePoint3d point = element.getVertexAt(i);
			pDumper.writeFieldValue("Vertex " + i, point);
		}
	}
}


class OdDgCurve2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgCurve2d element = (OdDgCurve2d)pObj;
		uint j = element.getVerticesCount();
		pDumper.writeFieldValue("Number of vertices", j);
		for (uint i = 0; i < j; i++)
		{
			OdGePoint2d point = element.getVertexAt(i);
			pDumper.writeFieldValue("Vertex " + i, point);
		}
	}
}



class OdDgCurve3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgCurve3d element = (OdDgCurve3d)pObj;
		uint j = element.getVerticesCount();
		pDumper.writeFieldValue("Number of vertices", j);
		for (uint i = 0; i < j; i++)
		{
			OdGePoint3d point = element.getVertexAt(i);
			pDumper.writeFieldValue("Vertex " + i, point);
		}
	}
}


class OdDgEllipse2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgEllipse2d element = (OdDgEllipse2d)pObj;
		pDumper.writeFieldValue("Primary axis", element.getPrimaryAxis());
		pDumper.writeFieldValue("Secondary axis", element.getSecondaryAxis());
		pDumper.writeFieldValue("Rotation", element.getRotationAngle());
		pDumper.writeFieldValue("Origin", element.getOrigin());
	}
}


class OdDgEllipse3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgEllipse3d element = (OdDgEllipse3d)pObj;
		pDumper.writeFieldValue("Primary axis", element.getPrimaryAxis());
		pDumper.writeFieldValue("Secondary axis", element.getSecondaryAxis());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Origin", element.origin());
	}
}

class OdDgArc2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgArc2d element = (OdDgArc2d)pObj;
		pDumper.writeFieldValue("Primary axis", element.getPrimaryAxis());
		pDumper.writeFieldValue("Secondary axis", element.getSecondaryAxis());
		pDumper.writeFieldValue("Rotation", element.getRotationAngle());
		pDumper.writeFieldValue("Origin", element.getOrigin());
		pDumper.writeFieldValue("Start angle", element.getStartAngle());
		pDumper.writeFieldValue("Sweep angle", element.getSweepAngle());
	}
}


class OdDgArc3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgArc3d element = (OdDgArc3d)pObj;
		pDumper.writeFieldValue("Primary axis", element.getPrimaryAxis());
		pDumper.writeFieldValue("Secondary axis", element.getSecondaryAxis());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Origin", element.getOrigin());
		pDumper.writeFieldValue("Start angle", element.getStartAngle());
		pDumper.writeFieldValue("Sweep angle", element.getSweepAngle());
	}
}

class OdDgConeDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgCone element = (OdDgCone)pObj;
		pDumper.writeFieldValue("Center 1", element.getCenter1());
		pDumper.writeFieldValue("Center 2", element.getCenter2());
		pDumper.writeFieldValue("Radius 1", element.getRadius1());
		pDumper.writeFieldValue("Radius 2", element.getRadius2());
		pDumper.writeFieldValue("Rotation", element.getRotation());
		pDumper.writeFieldValue("Is solid", element.isSolid());
		pDumper.writeFieldValue("Hole", element.getHbitFlag());
	}
}
class OdDgComplexStringDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgComplexString pComplElm = (OdDgComplexString)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
	}
}

class OdDgComplexShapeDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgComplexShape pComplElm = (OdDgComplexShape)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
	}
}

class OdDgPointString2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgPointString2d element = (OdDgPointString2d)pObj;
		uint j = element.getVerticesCount();
		pDumper.writeFieldValue("Number of vertices", j);
		pDumper.writeFieldValue("Continuous", element.getContinuousFlag());
		for (uint i = 0; i < j; i++)
		{
			pDumper.writeFieldValue("Vertex " + i, element.getVertexAt(i));
			pDumper.writeFieldValue("Rotation " + i, element.getRotationAt(i));
		}
	}
}

class OdDgPointString3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgPointString3d element = (OdDgPointString3d)pObj;
		uint j = element.getVerticesCount();
		pDumper.writeFieldValue("Number of vertices", j);
		pDumper.writeFieldValue("Continuous", element.getContinuousFlag());
		for (uint i = 0; i < j; i++)
		{
			pDumper.writeFieldValue("Vertex " + i, element.getVertexAt(i));
			pDumper.writeFieldValue("Rotation " + i, element.getRotationAt(i));
		}
	}
}
class OdDgDimensionDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgDimension element = (OdDgDimension)pObj;

		//simple fields
		pDumper.writeFieldValue("Tool type", element.getDimensionType());
		pDumper.writeFieldValue("Scale", element.getScaleFactor());
		pDumper.writeFieldValueHex("Flags", element.getFlags());

		{

			switch (element.getAlignment())
			{
				case OdDgDimension.PlacementAlignment.kPaArbitrary:
					pDumper.writeFieldValue("Placement Alignment", "kPaArbitrary\n");
					break;
				case OdDgDimension.PlacementAlignment.kPaDrawing:
					pDumper.writeFieldValue("Placement Alignment", "kPaDrawing\n");
					break;
				case OdDgDimension.PlacementAlignment.kPaTrue:
					pDumper.writeFieldValue("Placement Alignment", "kPaTrue\n");
					break;
				case OdDgDimension.PlacementAlignment.kPaView:
					pDumper.writeFieldValue("Placement Alignment", "kPaView\n");

					pDumper.writeFieldValue("Alignment View", element.getAlignmentView());
					break;
			};

			switch (element.getArrowHead())
			{
				case OdDgDimension.TerminatorArrowHeadType.kOpened:
					pDumper.writeFieldValue("Arrow Head Type", "kOpened\n");
					break;
				case OdDgDimension.TerminatorArrowHeadType.kClosed:
					pDumper.writeFieldValue("Arrow Head Type", "kClosed\n");
					break;
				case OdDgDimension.TerminatorArrowHeadType.kFilled:
					pDumper.writeFieldValue("Arrow Head Type", "kFilled\n");
					break;
			};

			if (element.Is3D())
			{
				OdGeQuaternion q = new OdGeQuaternion();
				element.getRotation(q);
				pDumper.writeFieldValue("Rotation", q);
			}
			else
			{
				OdGeMatrix2d q = new OdGeMatrix2d();
				element.getRotation(q);
				pDumper.writeFieldValue("Rotation", q);
			}
			/* TODO:

      #undef DUMPDIMFLAG
      #define DUMPDIMFLAG(flg) \
        pDumper.writeFieldValue( "Flag " + #flg, element.get##flg##Flag() );

        DUMPDIMFLAG(BoxText)
        DUMPDIMFLAG(CapsuleText)
        DUMPDIMFLAG(Centesimal)
        DUMPDIMFLAG(CrossCenter)
        DUMPDIMFLAG(Dual)
        DUMPDIMFLAG(Embed)
        DUMPDIMFLAG(Horizontal)
        DUMPDIMFLAG(Joiner)
        DUMPDIMFLAG(LeadingZero2)
        DUMPDIMFLAG(LeadingZeros)
        DUMPDIMFLAG(NoAutoTextLift)
        DUMPDIMFLAG(NoLevelSymb)
        DUMPDIMFLAG(RelDimLine)
        DUMPDIMFLAG(RelStat)
        DUMPDIMFLAG(StackFract)
        DUMPDIMFLAG(TextHeapPad)
        DUMPDIMFLAG(ThousandComma)
        DUMPDIMFLAG(Tolerance)
        DUMPDIMFLAG(Tolmode)
        DUMPDIMFLAG(TrailingZeros)
        DUMPDIMFLAG(TrailingZeros2)
        DUMPDIMFLAG(UnderlineText)*/
		}
		//
		//composite fields
		//

		//points
		{
			uint j = element.getPointsCount();
			pDumper.writeFieldValue("Number of points", j);
			for (uint i = 0; i < j; i++)
			{
				pDumper.writeFieldValue("Point " + i, element.getPoint(i));
			}
		}

		//text info
		{
			OdDgDimTextInfo textInfo = new OdDgDimTextInfo();
			element.getDimTextInfo(textInfo);
			pDumper.writeFieldValue("Text info", textInfo);
		}

		//text format
		{
			OdDgDimTextFormat textFormat = new OdDgDimTextFormat();
			element.getDimTextFormat(textFormat);
			pDumper.writeFieldValue("Text format", textFormat);
		}

		//geometry
		{
			OdDgDimGeometry geometry = new OdDgDimGeometry();
			element.getDimGeometry(geometry);
			pDumper.writeFieldValue("Geometry", geometry);
		}

		// Symbology
		{
			Int32 altLineStyle = element.getAltLineStyleEntryId();
			UInt32 altLineWeight = element.getAltLineWeight();
			UInt32 altColorIndex = element.getAltColorIndex();

			pDumper.writeFieldValue("Alternative LineStyle", altLineStyle);
			pDumper.writeFieldValue("Alternative LineWeight", altLineWeight);
			pDumper.writeFieldValue("Alternative ColorIndex", altColorIndex);
		}

		// tools

		pDumper.writeFieldValue("Tools:", element);

		//options
		{
			UInt32 iOptionsCount = 0;
			/* TODO:
      #undef  INC_DIM_OPTIONS_COUNT
      #define INC_DIM_OPTIONS_COUNT(name)                      \
      if( !element.getOption(OdDgDimOption.k##name).isNull() ) \
           iOptionsCount++;

      INC_DIM_OPTIONS_COUNT(Tolerance)
      INC_DIM_OPTIONS_COUNT(Terminators)
      INC_DIM_OPTIONS_COUNT(PrefixSymbol)
      INC_DIM_OPTIONS_COUNT(SuffixSymbol)
      INC_DIM_OPTIONS_COUNT(DiameterSymbol)
      INC_DIM_OPTIONS_COUNT(PrefixSuffix)
      INC_DIM_OPTIONS_COUNT(PrimaryUnits)
      INC_DIM_OPTIONS_COUNT(SecondaryUnits)
      INC_DIM_OPTIONS_COUNT(TerminatorSymbology)
      INC_DIM_OPTIONS_COUNT(View)
      INC_DIM_OPTIONS_COUNT(AlternatePrimaryUnits)
      INC_DIM_OPTIONS_COUNT(Offset)
      INC_DIM_OPTIONS_COUNT(AlternateSecondaryUnits)
      INC_DIM_OPTIONS_COUNT(AlternatePrefixSymbol)
      INC_DIM_OPTIONS_COUNT(AlternateSuffixSymbol)
      INC_DIM_OPTIONS_COUNT(ProxyCell)
      */
			pDumper.writeFieldValue("Number of options", iOptionsCount);

			//OdDgDimOption pDimOptions;
			iOptionsCount = 0;
			/* TODO:
    #undef  DUMP_DIM_OPTIONS
    #define DUMP_DIM_OPTIONS(name)                                 \
      pDimOptions = element.getOption(OdDgDimOption.k##name);  \
      if (pDimOptions != null)                                \
      {                                                          \
        String strOptionNum;                                   \
        strOptionNum.format("Dimension option %d", iOptionsCount ); \
        pDumper.writeFieldValue( strOptionNum, pDimOptions ); \
        iOptionsCount++; \
      }

      DUMP_DIM_OPTIONS(Tolerance)
      DUMP_DIM_OPTIONS(Terminators)
      DUMP_DIM_OPTIONS(PrefixSymbol)
      DUMP_DIM_OPTIONS(SuffixSymbol)
      DUMP_DIM_OPTIONS(DiameterSymbol)
      DUMP_DIM_OPTIONS(PrefixSuffix)
      DUMP_DIM_OPTIONS(PrimaryUnits)
      DUMP_DIM_OPTIONS(SecondaryUnits)
      DUMP_DIM_OPTIONS(TerminatorSymbology)
      DUMP_DIM_OPTIONS(View)
      DUMP_DIM_OPTIONS(AlternatePrimaryUnits)
      DUMP_DIM_OPTIONS(Offset)
      DUMP_DIM_OPTIONS(AlternateSecondaryUnits)
      DUMP_DIM_OPTIONS(AlternatePrefixSymbol)
      DUMP_DIM_OPTIONS(AlternateSuffixSymbol)
      DUMP_DIM_OPTIONS(ProxyCell)
    }*/
		}
	}
}
class OdDgMultilineDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgMultiline element = (OdDgMultiline)pObj;

		//simple fields
		pDumper.writeFieldValue("Origin cap angle", element.getOriginCapAngle());
		pDumper.writeFieldValue("End cap angle", element.getEndCapAngle());
		{
			OdGeVector3d vector;

			vector = element.getZVector();
			pDumper.writeFieldValue("Z vector", vector);
		}
		pDumper.writeFieldValue("Is closed", element.getClosedFlag());

		//symbologies
		{
			OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

			element.getOriginCap(symbology);
			pDumper.writeFieldValue("Origin cap", symbology);
			element.getMiddleCap(symbology);
			pDumper.writeFieldValue("Joint cap", symbology);
			element.getEndCap(symbology);
			pDumper.writeFieldValue("End cap", symbology);
		}

		//points
		{
			UInt32 j = element.getPointsCount();
			pDumper.writeFieldValue("Number of points", j);
			for (int i = 0; i < j; i++)
			{
				OdDgMultilinePoint point = new OdDgMultilinePoint();
				element.getPoint(i, point);
				pDumper.writeFieldValue("Point " + i, point);
			}
		}

		//profiles
		{
			UInt32 j = element.getProfilesCount();
			pDumper.writeFieldValue("Number of profiles", j);
			for (int i = 0; i < j; i++)
			{
				OdDgMultilineProfile profile = new OdDgMultilineProfile();
				element.getProfile(i, profile);
				pDumper.writeFieldValue("Profile " + i, profile);
			}
		}
	}
}

class OdDgBSplineCurve2dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgBSplineCurve2d element = (OdDgBSplineCurve2d)pObj;

		//simple fields
		pDumper.writeFieldValue("Order", element.getOrder());
		pDumper.writeFieldValue("Display curve", element.getCurveDisplayFlag());
		pDumper.writeFieldValue("Display polygon", element.getPolygonDisplayFlag());
		pDumper.writeFieldValue("Closed", element.getClosedFlag());

		if (element.hasFitData())
		{
			pDumper.writeFieldValue("Construction type", "kDefinedByFitPoints");
			pDumper.writeFieldValue("Natural tangents", element.getNaturalTangentsFlag());
			pDumper.writeFieldValue("Chord length tangents", element.getChordLenTangentsFlag());
			pDumper.writeFieldValue("Collinear tangents", element.getColinearTangentsFlag());

			OdGePoint2dArray fitPoints = new OdGePoint2dArray();
			UInt32 uOrder = 0;
			bool bTangentExists = false;
			OdGeVector2d vrStartTangent = new OdGeVector2d();
			OdGeVector2d vrEndTangent = new OdGeVector2d();
			bool bUniformKnots = false;
			element.getFitData(fitPoints, out uOrder, out bTangentExists, vrStartTangent, vrEndTangent, out bUniformKnots);

			OdGeKnotVector vrKnots = element.getKnots();

			pDumper.writeFieldValue("Num Fit Points", fitPoints.Count);

			for (int i = 0; i < fitPoints.Count; i++)
			{
				string strFitPtsName = "  Point " + i.ToString();
				pDumper.writeFieldValue(strFitPtsName, fitPoints[i]);
			}

			pDumper.writeFieldValue("Start Tangent", vrStartTangent);
			pDumper.writeFieldValue("End Tangent", vrEndTangent);
			pDumper.writeFieldValue("Uniform Knots Flag", bUniformKnots);

			pDumper.writeFieldValue("Num Knots", vrKnots.length());

			for (int j = 0; j < vrKnots.length(); j++)
			{
				String strKnotName = "  Knot " + j.ToString();
				pDumper.writeFieldValue(strKnotName, vrKnots[j]);
			}
		}
		else
		{
			pDumper.writeFieldValue("Construction type", "kDefinedByNurbsData");
			pDumper.writeFieldValue("Rational", element.isRational());

			OdGePoint2dArray arrCtrlPts = new OdGePoint2dArray();
			OdGeKnotVector vrKnots = new OdGeKnotVector();
			OdGeDoubleArray arrWeights = new OdGeDoubleArray();
			UInt32 uOrder = 0;
			bool bClosed = false;
			bool bRational = false;

			element.getNurbsData(out uOrder, out bRational, out bClosed, arrCtrlPts, vrKnots, arrWeights);

			pDumper.writeFieldValue("Num Control Points", arrCtrlPts.Count);

			for (int i = 0; i < arrCtrlPts.Count; i++)
			{
				String strCtrlPtsName = "  Point %d" + i.ToString();
				pDumper.writeFieldValue(strCtrlPtsName, arrCtrlPts[i]);
			}

			pDumper.writeFieldValue("Num Knots", vrKnots.length());

			for (int j = 0; j < vrKnots.length(); j++)
			{
				String strKnotName = "  Knot %d" + j.ToString();
				pDumper.writeFieldValue(strKnotName, vrKnots[j]);
			}

			if (bRational)
			{
				pDumper.writeFieldValue("Num Weights", arrWeights.Count);

				for (int k = 0; k < arrWeights.Count; k++)
				{
					String strWeightName = "  Weight %d" + k.ToString();
					pDumper.writeFieldValue(strWeightName, arrWeights[k]);
				}
			}
		}
	}
}
class OdDgBSplineCurve3dDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgBSplineCurve3d element = (OdDgBSplineCurve3d)pObj;

		//simple fields
		pDumper.writeFieldValue("Order", element.getOrder());
		pDumper.writeFieldValue("Display curve", element.getCurveDisplayFlag());
		pDumper.writeFieldValue("Display polygon", element.getPolygonDisplayFlag());
		pDumper.writeFieldValue("Closed", element.getClosedFlag());

		if (element.hasFitData())
		{
			pDumper.writeFieldValue("Construction type", "kDefinedByFitPoints");
			pDumper.writeFieldValue("Natural tangents", element.getNaturalTangentsFlag());
			pDumper.writeFieldValue("Chord length tangents", element.getChordLenTangentsFlag());
			pDumper.writeFieldValue("Collinear tangents", element.getColinearTangentsFlag());

			OdGePoint3dArray fitPoints = new OdGePoint3dArray();
			UInt32 uOrder = 0;
			bool bTangentExists = false;
			OdGeVector3d vrStartTangent = new OdGeVector3d();
			OdGeVector3d vrEndTangent = new OdGeVector3d();
			bool bUniformKnots = false;
			element.getFitData(fitPoints, out uOrder, out bTangentExists, vrStartTangent, vrEndTangent, out bUniformKnots);

			OdGeKnotVector vrKnots = element.getKnots();

			pDumper.writeFieldValue("Num Fit Points", fitPoints.Count);

			for (int i = 0; i < fitPoints.Count; i++)
			{
				String strFitPtsName = "  Point " + i.ToString();
				pDumper.writeFieldValue(strFitPtsName, fitPoints[i]);
			}

			pDumper.writeFieldValue("Start Tangent", vrStartTangent);
			pDumper.writeFieldValue("End Tangent", vrEndTangent);
			pDumper.writeFieldValue("Uniform Knots Flag", bUniformKnots);

			pDumper.writeFieldValue("Num Knots", vrKnots.length());

			for (int j = 0; j < vrKnots.length(); j++)
			{
				String strKnotName = "  Knot " + j.ToString();
				pDumper.writeFieldValue(strKnotName, vrKnots[j]);
			}
		}
		else
		{
			pDumper.writeFieldValue("Construction type", "kDefinedByNurbsData");
			pDumper.writeFieldValue("Rational", element.isRational());

			OdGePoint3dArray arrCtrlPts = new OdGePoint3dArray();
			OdGeKnotVector vrKnots = new OdGeKnotVector();
			OdGeDoubleArray arrWeights = new OdGeDoubleArray();
			UInt32 uOrder = 0;
			bool bClosed = false;
			bool bRational = false;

			element.getNurbsData(out uOrder, out bRational, out bClosed, arrCtrlPts, vrKnots, arrWeights);

			pDumper.writeFieldValue("Num Control Points", arrCtrlPts.Count);

			for (int i = 0; i < arrCtrlPts.Count; i++)
			{
				String strCtrlPtsName = "  Point " + i.ToString();
				pDumper.writeFieldValue(strCtrlPtsName, arrCtrlPts[i]);
			}

			pDumper.writeFieldValue("Num Knots", vrKnots.length());

			for (int j = 0; j < vrKnots.length(); j++)
			{
				String strKnotName = "  Knot " + j.ToString();
				pDumper.writeFieldValue(strKnotName, vrKnots[j]);
			}

			if (bRational)
			{
				pDumper.writeFieldValue("Num Weights", arrWeights.Count);

				for (int k = 0; k < arrWeights.Count; k++)
				{
					String strWeightName = "  Weight " + k.ToString();
					pDumper.writeFieldValue(strWeightName, arrWeights[k]);
				}
			}
		}
	}
}
class OdDgSurfaceDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
	}
}
class OdDgSolidDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
	}
}
class OdDgMeshFaceLoopsDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		//   OdDgMeshFaceLoops pMesh = pObj;
		// 
		//   UInt32 nFaces = pMesh.getFacesNumber();
		// 
		//   for( UInt32 i = 0; i < nFaces; i++ )
		//   {
		//     OdDgMeshFaceLoops.VerticesArray pointsData;
		// 
		//     pDumper.writeFieldValue("  Face number", i );
		// 
		//     pMesh.getFace( i, pointsData);
		//     
		//     for( UInt32 j = 0; j < pointsData.Count; j++ )
		//     {
		//       pDumper.writeFieldValue("    Point number", j );
		//       pDumper.writeFieldValue("    Point value", pMesh.getPoint( pointsData[j].m_pointIndex ) );
		// 
		//       if( pMesh.getUseColorTableIndexesFlag() )
		//       {
		//         pDumper.writeFieldValue("    Color Index", pointsData[j].m_colorIndex );
		//       }
		// 
		//       if( pMesh.getUseDoubleColorsFlag() )
		//       {
		//         String strColors;
		//         strColors.format("%d %d %d", pointsData[j].m_dColorRGB[0], pointsData[j].m_dColorRGB[1], pointsData[j].m_dColorRGB[2] );
		//         pDumper.writeFieldValue("    Double colors", strColors );
		//       }
		// 
		//       if( pMesh.getUseNormalsFlag() )
		//       {
		//         pDumper.writeFieldValue("    Normal", pointsData[j].m_vrNormal );
		//       }
		// 
		//       if( pMesh.getUseTextureCoordinatesFlag() )
		//       {
		//         pDumper.writeFieldValue("    Texture Coordinate", pointsData[j].m_texCoords );
		//       }
		//     }
		//   }
	}
}

class OdDgRasterAttachmentHeaderDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgRasterAttachmentHeader element = (OdDgRasterAttachmentHeader)pObj;


		//simple fields
		{
			OdGePoint3d origin = new OdGePoint3d();
			OdGeVector3d u = new OdGeVector3d();
			OdGeVector3d v = new OdGeVector3d();
			//these values are logged later with OdDgRasterAttachmentComponentGeo pObj
			element.getOrientation(origin, u, v);
			pDumper.writeFieldValue("Origin", origin);
		}
		pDumper.writeFieldValue("Extent", element.getExtent());
		pDumper.writeFieldValue("Display gamma", element.getDisplayGamma());
		pDumper.writeFieldValue("Plot gamma", element.getPlotGamma());
		pDumper.writeFieldValue("Apply rotation", element.getApplyRotationFlag());
		pDumper.writeFieldValue("Clipping", element.getClippingFlag());
		pDumper.writeFieldValue("Plot", element.getPlotFlag());
		pDumper.writeFieldValue("Invert", element.getInvertFlag());
		{
			for (int i = 1; i <= 8; i++)
			{
				pDumper.writeFieldValue("View " + i, element.getViewFlag(i));
			}
		}
	}
}

class OdDgRasterHeader2dDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgRasterHeader2d pComplElm = (OdDgRasterHeader2d)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgRasterHeader2d element = (OdDgRasterHeader2d)pObj;

		//simple fields
		pDumper.writeFieldValue("Right justified", element.getRightJustifiedFlag());
		pDumper.writeFieldValue("Lower justified", element.getLowerJustifiedFlag());
		pDumper.writeFieldValue("Horizontal data", element.getHorizontalDataFlag());
		pDumper.writeFieldValue("Color", element.getColorFlag());
		pDumper.writeFieldValue("Transparent", element.getTransparentFlag());
		pDumper.writeFieldValue("Positive", element.getPositiveFlag());
		pDumper.writeFieldValue("Raster format", element.getFormat());
		pDumper.writeFieldValue("Foreground", element.getForeground());
		pDumper.writeFieldValue("Background", element.getBackground());
		pDumper.writeFieldValue("X extent", element.getXExtent());
		pDumper.writeFieldValue("Y extent", element.getYExtent());
		pDumper.writeFieldValue("Scale", element.getScale());
		{
			OdGePoint3d origin = element.getOrigin();
			pDumper.writeFieldValue("Origin", origin);
		}
	}
}
class OdDgRasterHeader3dDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgRasterHeader3d pComplElm = (OdDgRasterHeader3d)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgRasterHeader3d element = (OdDgRasterHeader3d)pObj;

		//simple fields
		pDumper.writeFieldValue("Right justified", element.getRightJustifiedFlag());
		pDumper.writeFieldValue("Lower justified", element.getLowerJustifiedFlag());
		pDumper.writeFieldValue("Horizontal data", element.getHorizontalDataFlag());
		pDumper.writeFieldValue("Color", element.getColorFlag());
		pDumper.writeFieldValue("Transparent", element.getTransparentFlag());
		pDumper.writeFieldValue("Positive", element.getPositiveFlag());
		pDumper.writeFieldValue("Raster format", element.getFormat());
		pDumper.writeFieldValue("Foreground", element.getForeground());
		pDumper.writeFieldValue("Background", element.getBackground());
		pDumper.writeFieldValue("X extent", element.getXExtent());
		pDumper.writeFieldValue("Y extent", element.getYExtent());
		pDumper.writeFieldValue("Scale", element.getScale());
		{
			OdGePoint3d origin = element.getOrigin();
			pDumper.writeFieldValue("Origin", origin);
		}
	}
}

class OdDgRasterComponentDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgRasterComponent element = (OdDgRasterComponent)pObj;

		//simple fields
		pDumper.writeFieldValue("Right justified", element.getRightJustifiedFlag());
		pDumper.writeFieldValue("Lower justified", element.getLowerJustifiedFlag());
		pDumper.writeFieldValue("Horizontal data", element.getHorizontalDataFlag());
		pDumper.writeFieldValue("Color", element.getColorFlag());
		pDumper.writeFieldValue("Transparent", element.getTransparentFlag());
		pDumper.writeFieldValue("Positive", element.getPositiveFlag());
		pDumper.writeFieldValue("Raster format", element.getFormat());
		pDumper.writeFieldValue("Foreground", element.getForeground());
		pDumper.writeFieldValue("Background", element.getBackground());
		pDumper.writeFieldValue("Offset X", element.getOffsetX());
		pDumper.writeFieldValue("Offset Y", element.getOffsetY());
		pDumper.writeFieldValue("Number of pixels", element.getNumberOfPixels());
	}
}
class OdDgTagElementDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgTagElement element = (OdDgTagElement)pObj;

		//simple fields
		pDumper.writeFieldValue("Origin", element.getOrigin());
		pDumper.writeFieldValue("Offset", element.getOffset());
		pDumper.writeFieldValue("Rotation (3d)", element.getRotation3d());
		pDumper.writeFieldValue("Associated", element.getAssociatedFlag());
		pDumper.writeFieldValue("Offset is used", element.getOffsetUsedFlag());
		{
			OdDgElementId setId = new OdDgElementId();
			UInt16 definitionId;
			element.getTagDefinitionId(setId, out definitionId);
			pDumper.writeFieldValue("Id of the set", setId);
			pDumper.writeFieldValue("Entry Id of the definition", definitionId);

			if (!setId.isNull())
			{
				OdDgTagDefinitionSet pTagSet = setId.safeOpenObject() as OdDgTagDefinitionSet;
				if (pTagSet != null)
				{
					pDumper.writeFieldValue("TagDefinitionsSet Name", pTagSet.getName());
					OdDgTagDefinition pTagDefinition = pTagSet.getByEntryId(definitionId);
					if (pTagDefinition != null)
					{
						pDumper.writeFieldValue("TagDefinition Name", pTagDefinition.getName());
						pDumper.writeFieldValue("TagDefinition Type", pTagDefinition.getType());
					}
				}
			}
		}
		pDumper.writeFieldValue("Id of the associated element", element.getAssociationId());
		pDumper.writeFieldValue("Freeze group", element.getFreezeGroup());

		switch (element.getDataType())
		{
			case OdDgTagDefinition.Type.kChar:
				pDumper.writeFieldValue("Type", "char");
				pDumper.writeFieldValue("Value", element.getString());
				break;
			case OdDgTagDefinition.Type.kInt16:
				pDumper.writeFieldValue("Type", "int16");
				pDumper.writeFieldValue("Value", element.getInt16());
				break;
			case OdDgTagDefinition.Type.kInt32:
				pDumper.writeFieldValue("Type", "int32");
				pDumper.writeFieldValue("Value", element.getInt32());
				break;
			case OdDgTagDefinition.Type.kDouble:
				pDumper.writeFieldValue("Type", "Double");
				pDumper.writeFieldValue("Value", element.getDouble());
				break;
			case OdDgTagDefinition.Type.kBinary:
				pDumper.writeFieldValue("Type", "Binary");
				pDumper.writeFieldValue("Size", element.getBinarySize());
				break;
			default:
				Debug.Fail("Invalid data type");
				break;
		}

		pDumper.writeFieldValue("Use interChar spacing", element.getUseInterCharSpacingFlag());
		pDumper.writeFieldValue("Fixed width spacing", element.getFixedWidthSpacingFlag());
		pDumper.writeFieldValue("Underlined", element.getUnderlineFlag());
		pDumper.writeFieldValue("Use slant", element.getUseSlantFlag());
		pDumper.writeFieldValue("Vertical", element.getVerticalFlag());
		pDumper.writeFieldValue("Right-to-left", element.getRightToLeftFlag());
		pDumper.writeFieldValue("Reverse MLine", element.getReverseMlineFlag());
		pDumper.writeFieldValue("Kerning", element.getKerningFlag());
		pDumper.writeFieldValue("Use codepage", element.getUseCodepageFlag());
		pDumper.writeFieldValue("Use SHX big font", element.getUseShxBigFontFlag());
		pDumper.writeFieldValue("Subscript", element.getSubscriptFlag());
		pDumper.writeFieldValue("Superscript", element.getSuperscriptFlag());
		pDumper.writeFieldValue("Use text style", element.getUseTextStyleFlag());
		pDumper.writeFieldValue("Overlined", element.getOverlineFlag());
		pDumper.writeFieldValue("Bold", element.getBoldFlag());
		pDumper.writeFieldValue("Full justification", element.getFullJustificationFlag());
		pDumper.writeFieldValue("ACAD interChar spacing", element.getAcadInterCharSpacingFlag());
		pDumper.writeFieldValue("Backwards", element.getBackwardsFlag());
		pDumper.writeFieldValue("Upside down", element.getUpsideDownFlag());
		pDumper.writeFieldValue("ACAD fitted text", element.getAcadFittedTextFlag());

		pDumper.writeFieldValue("Slant", element.getSlant());
		pDumper.writeFieldValue("Character spacing", element.getCharacterSpacing());
		pDumper.writeFieldValue("Underline spacing", element.getUnderlineSpacing());
		pDumper.writeFieldValue("Length multiplier", element.getLengthMultiplier());
		pDumper.writeFieldValue("Height multiplier", element.getHeightMultiplier());
		pDumper.writeFieldValue("Text style ID", element.getTextStyleEntryId());
		pDumper.writeFieldValue("SHX big font", element.getShxBigFont());
		pDumper.writeFieldValue("Font ID", element.getFontEntryId());
		pDumper.writeFieldValue("Justification", element.getJustification());
		pDumper.writeFieldValue("Codepage", element.getCodepage());
	}
}
class OdDgCellHeader2dDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgCellHeader2d pComplElm = (OdDgCellHeader2d)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgCellHeader2d pCell = (OdDgCellHeader2d)pObj;

		pDumper.writeFieldValue("Name", pCell.getName());
		pDumper.writeFieldValue("Origin", pCell.getOrigin());
	}
}
class OdDgCellHeader3dDumperPE : OdDgGraphicsElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgCellHeader3d pComplElm = (OdDgCellHeader3d)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgCellHeader3d pCell = (OdDgCellHeader3d)pObj;

		pDumper.writeFieldValue("Name", pCell.getName());
		pDumper.writeFieldValue("Origin", pCell.getOrigin());
	}
}
class OdDgDistantLightDumperPE : OdDgCellHeader3dDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLightDistant pLight = (OdDgLightDistant)pObj;

		pDumper.writeFieldValue("Light name", pLight.getLightName());
		pDumper.writeFieldValue("On Flag", pLight.getOnFlag());
		pDumper.writeFieldValue("Intensity", pLight.getIntensity());
		pDumper.writeFieldValue("Brightness", pLight.getBrightness());
		pDumper.writeFieldValue("Shadows On Flag", pLight.getShadowOnFlag());
		pDumper.writeFieldValue("Shadow Resolution", pLight.getShadowResolution());
		pDumper.writeFieldValue("Color Red", pLight.getColorRed());
		pDumper.writeFieldValue("Color Green", pLight.getColorGreen());
		pDumper.writeFieldValue("Color Blue", pLight.getColorBlue());
	}
}
class OdDgPointLightDumperPE : OdDgCellHeader3dDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLightPoint pLight = (OdDgLightPoint)pObj;

		pDumper.writeFieldValue("Light name", pLight.getLightName());
		pDumper.writeFieldValue("On Flag", pLight.getOnFlag());
		pDumper.writeFieldValue("Intensity", pLight.getIntensity());
		pDumper.writeFieldValue("Brightness", pLight.getBrightness());
		pDumper.writeFieldValue("Shadows On Flag", pLight.getShadowOnFlag());
		pDumper.writeFieldValue("Shadow Resolution", pLight.getShadowResolution());
		pDumper.writeFieldValue("Color Red", pLight.getColorRed());
		pDumper.writeFieldValue("Color Green", pLight.getColorGreen());
		pDumper.writeFieldValue("Color Blue", pLight.getColorBlue());
		pDumper.writeFieldValue("Attenuate On Flag", pLight.getAttenuateOnFlag());
		pDumper.writeFieldValue("Attenuation Distance", pLight.getAttenuationDistance());
		pDumper.writeFieldValue("IES Data On Flag", pLight.getIESDataOnFlag());
		pDumper.writeFieldValue("IES Rotation", pLight.getIESRotation());
		pDumper.writeFieldValue("IES Filename", pLight.getIESFilename());
	}
}
class OdDgSpotLightDumperPE : OdDgCellHeader3dDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLightSpot pLight = (OdDgLightSpot)pObj;

		pDumper.writeFieldValue("Light name", pLight.getLightName());
		pDumper.writeFieldValue("On Flag", pLight.getOnFlag());
		pDumper.writeFieldValue("Intensity", pLight.getIntensity());
		pDumper.writeFieldValue("Brightness", pLight.getBrightness());
		pDumper.writeFieldValue("Shadows On Flag", pLight.getShadowOnFlag());
		pDumper.writeFieldValue("Shadow Resolution", pLight.getShadowResolution());
		pDumper.writeFieldValue("Color Red", pLight.getColorRed());
		pDumper.writeFieldValue("Color Green", pLight.getColorGreen());
		pDumper.writeFieldValue("Color Blue", pLight.getColorBlue());
		pDumper.writeFieldValue("Attenuate On Flag", pLight.getAttenuateOnFlag());
		pDumper.writeFieldValue("Attenuation Distance", pLight.getAttenuationDistance());
		pDumper.writeFieldValue("IES Data On Flag", pLight.getIESDataOnFlag());
		pDumper.writeFieldValue("IES Rotation", pLight.getIESRotation());
		pDumper.writeFieldValue("IES Filename", pLight.getIESFilename());
		pDumper.writeFieldValue("Cone Angle", pLight.getConeAngle());
		pDumper.writeFieldValue("Delta Angle", pLight.getDeltaAngle());
	}
}
class OdDgAreaLightDumperPE : OdDgCellHeader3dDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLightArea pLight = (OdDgLightArea)pObj;

		pDumper.writeFieldValue("Light name", pLight.getLightName());
		pDumper.writeFieldValue("On Flag", pLight.getOnFlag());
		pDumper.writeFieldValue("Intensity", pLight.getIntensity());
		pDumper.writeFieldValue("Brightness", pLight.getBrightness());
		pDumper.writeFieldValue("Shadows On Flag", pLight.getShadowOnFlag());
		pDumper.writeFieldValue("Shadow Resolution", pLight.getShadowResolution());
		pDumper.writeFieldValue("Color Red", pLight.getColorRed());
		pDumper.writeFieldValue("Color Green", pLight.getColorGreen());
		pDumper.writeFieldValue("Color Blue", pLight.getColorBlue());
		pDumper.writeFieldValue("Attenuate On Flag", pLight.getAttenuateOnFlag());
		pDumper.writeFieldValue("Attenuation Distance", pLight.getAttenuationDistance());
		pDumper.writeFieldValue("IES Data On Flag", pLight.getIESDataOnFlag());
		pDumper.writeFieldValue("IES Rotation", pLight.getIESRotation());
		pDumper.writeFieldValue("IES Filename", pLight.getIESFilename());
		pDumper.writeFieldValue("Sample Count", pLight.getSampleCount());
	}
}
class OdDgSkyOpeningLightDumperPE : OdDgCellHeader3dDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgLightOpenSky pLight = (OdDgLightOpenSky)pObj;

		pDumper.writeFieldValue("Light name", pLight.getLightName());
		pDumper.writeFieldValue("On Flag", pLight.getOnFlag());
		pDumper.writeFieldValue("Min samples", pLight.getMinSamples());
		pDumper.writeFieldValue("Max samples", pLight.getMaxSamples());
	}
}
class OdDgBSplineSurfaceDumperPE : OdDgGraphicsElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);

		OdDgBSplineSurface element = (OdDgBSplineSurface)pObj;

		//simple fields
		byte uOrderU = 0;
		byte uOrderV = 0;
		bool bRational = false;
		bool bClosedInU = false;
		bool bClosedInV = false;
		int nCtrlPtsInU = 0;
		int nCtrlPtsInV = 0;
		OdGePoint3dArray arrCtrlPts = new OdGePoint3dArray();
		OdGeDoubleArray arrWeights = new OdGeDoubleArray();
		OdGeKnotVector arrKnotsU = new OdGeKnotVector();
		OdGeKnotVector arrKnotsV = new OdGeKnotVector();
		uint nRulesU = 0;
		uint nRulesV = 0;

		element.get(out uOrderU, out uOrderV, out bRational, out bClosedInU, out bClosedInV, out nCtrlPtsInU, out nCtrlPtsInV, arrCtrlPts,
								arrWeights, arrKnotsU, arrKnotsV);
		element.getNumberOfSpansInU(out nRulesU);
		element.getNumberOfSpansInV(out nRulesV);

		//simple fields
		pDumper.writeFieldValue("Order U", uOrderU);
		pDumper.writeFieldValue("Order V", uOrderV);
		pDumper.writeFieldValue("Closed U", bClosedInU);
		pDumper.writeFieldValue("Closed V", bClosedInV);
		pDumper.writeFieldValue("Display curve", element.getSurfaceDisplayFlag());
		pDumper.writeFieldValue("Display polygon", element.getControlNetDisplayFlag());
		pDumper.writeFieldValue("Rational", bRational);
		pDumper.writeFieldValue("Number of rules U", nRulesU);
		pDumper.writeFieldValue("Number of rules V", nRulesV);
		pDumper.writeFieldValue("Number of poles U", nCtrlPtsInU);
		pDumper.writeFieldValue("Number of poles V", nCtrlPtsInV);
		pDumper.writeFieldValue("Number of knots U", arrKnotsU.length());
		pDumper.writeFieldValue("Number of knots V", arrKnotsV.length());
		pDumper.writeFieldValue("Number of boundaries", element.getBoundariesCount());
		pDumper.writeFieldValue("Hole", element.getHoleFlag());

		for (uint nBoundariesCount = element.getBoundariesCount(), i = 0;
				i < nBoundariesCount;
				i++
				)
		{
			//writeShift();
			//fwprintf( DumpStream, L"Boundary %d:\n", i );
			pDumper.writeFieldValue("Boundary ", i.ToString());
			OdGePoint2dArray arrBoundaryPts = new OdGePoint2dArray();
			element.getBoundary(i, arrBoundaryPts);
			pDumper.writeFieldValue("Number of boundary vertices", arrBoundaryPts.Count);
			for (int BoundaryVerticesCount = arrBoundaryPts.Count, j = 0;
							j < BoundaryVerticesCount;
							j++
							)
			{
				pDumper.writeFieldValue("Vertex", arrBoundaryPts[j]);
			}
		}
	}
}

class OdDgProxyElementDumperPE : OdDgElementDumperPE
{
	protected override void dumpData(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dumpData(pObj, pDumper);
		OdDgElement pElm = (OdDgElement)pObj;
		pDumper.writeFieldValue("Element Type", pElm.getElementType());
		pDumper.writeFieldValue("Element SubType", pElm.getElementSubType());

		OdDgProxyElement pProxyElm = (OdDgProxyElement)pObj;

		OdBinaryData binData = new OdBinaryData();
		pProxyElm.getData(binData);


		pDumper.writeFieldValue("Data Size", (UInt32)(binData.Count));

		String strDataLine = "";
		String strData = "";
		UInt32 nData = 0;
		int uDataLine = 0;
		//TODO: binData to UInt16Array.
		for (Int32 l = 0; l < binData.Count; l++)
		{
			String strByte = "";
			strByte = String.Format("{0:X2}, ", binData[l]);
			strData += strByte;
			nData++;

			if (nData == 8)
			{
				strData = strData.Substring(0, strData.Length - 2);
				strDataLine = String.Format("  Line {0}", uDataLine);
				uDataLine++;

				pDumper.writeFieldValue(strDataLine, strData);
				strData = "";
				nData = 0;
			}
		}
		if (strData != "")
		{
			strData = strData.Substring(0, strData.Length - 2);
			strDataLine = String.Format("  Line {0}", uDataLine);

			pDumper.writeFieldValue(strDataLine, strData);
		}
	}
}
class OdDgComplexProxyElementDumperPE : OdDgProxyElementDumperPE
{
	public override OdDgElementIterator createIterator(OdDgElement pElm, bool atBeginning, bool skipDeleted)
	{
		OdDgComplexProxyElement pComplElm = (OdDgComplexProxyElement)pElm;
		return pComplElm.createIterator(atBeginning, skipDeleted);
	}
}

class OdDgCompoundLineStyleResourceDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgCompoundLineStyleResource pRes = (OdDgCompoundLineStyleResource)pObj;

		pDumper.writeFieldValue("Description", pRes.getDescription());

		for (uint i = 0; i < pRes.getComponentCount(); ++i)
		{
			OdDgCompoundLineStyleComponentInfo cmpInfo = new OdDgCompoundLineStyleComponentInfo();
			pRes.getComponent(i, cmpInfo);
			pDumper.writeFieldValue("Component (index)", i);
			pDumper.writeFieldValue(String.Format("  Component {0} Type", i), cmpInfo.getComponentType());
			pDumper.writeFieldValue(String.Format("  Component {0} Offset", i), cmpInfo.getComponentOffset());
			if (cmpInfo.getComponentHandleId() == 0)
			{
				pDumper.writeFieldValueHex(String.Format("  Component {0} Entry ID", i), cmpInfo.getComponentEntryId());
			}
			else
			{
				pDumper.writeFieldValueHex(String.Format("  Component {0} Handle ID", i), cmpInfo.getComponentHandleId());
			}
		}
	}
}
class OdDgLineCodeResourceDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgLineCodeResource pRes = (OdDgLineCodeResource)pObj;

		pDumper.writeFieldValue("Description", pRes.getDescription());
		pDumper.writeFieldValue("Phase", pRes.getPhase());
		pDumper.writeFieldValue("Auto Phase Flag", pRes.getAutoPhaseFlag());
		pDumper.writeFieldValue("Single Segment Mode Flag", pRes.getSingleSegmentModeFlag());
		pDumper.writeFieldValue("Center Stretch Phase Mode Flag", pRes.getCenterStretchPhaseModeFlag());
		pDumper.writeFieldValue("Use Iteration Limit Flag", pRes.getUseIterationLimitFlag());
		pDumper.writeFieldValue("Max Iterations", pRes.getMaxIterations());

		for (uint i = 0; i < pRes.getStrokeCount(); ++i)
		{
			OdDgLineCodeResourceStrokeData strokeData = new OdDgLineCodeResourceStrokeData();
			pRes.getStroke(i, strokeData);

			pDumper.writeFieldValue("Stroke (index)", i);
			pDumper.writeFieldValue("  Length", strokeData.getLength());
			pDumper.writeFieldValue("  Width", strokeData.getStartWidth());
			pDumper.writeFieldValue("  EndWidth", strokeData.getEndWidth());
			pDumper.writeFieldValue("  Dash Flag", strokeData.getDashFlag());
			pDumper.writeFieldValue("  ByPass Corner Flag", strokeData.getByPassCornerFlag());
			pDumper.writeFieldValue("  Can Be Scaled Flag", strokeData.getCanBeScaledFlag());
			pDumper.writeFieldValue("  Invert Stroke In First Code Flag", strokeData.getInvertStrokeInFirstCodeFlag());
			pDumper.writeFieldValue("  Invert Stroke In Last Code", strokeData.getInvertStrokeInLastCodeFlag());
			pDumper.writeFieldValue("  Width Mode", strokeData.getWidthMode());
			pDumper.writeFieldValue("  Decreasing Taper Flag", strokeData.getDecreasingTaperFlag());
			pDumper.writeFieldValue("  Increasing Taper Flag", strokeData.getIncreasingTaperFlag());
			pDumper.writeFieldValue("  Caps Type", strokeData.getCapsType());
		}
	}
}
class OdDgLinePointResourceDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgLinePointResource pRes = (OdDgLinePointResource)pObj;

		pDumper.writeFieldValue("Description", pRes.getDescription());

		pDumper.writeFieldValue("Base Pattern Type", pRes.getBasePatternType());

		if (pRes.getBasePatternHandleId() == 0)
		{
			pDumper.writeFieldValue("Base Pattern Entry Id", pRes.getBasePatternEntryId());
		}
		else
		{
			pDumper.writeFieldValueHex("Base Pattern Handle Id", pRes.getBasePatternHandleId());
		}

		for (uint i = 0; i < pRes.getSymbolCount(); ++i)
		{
			pDumper.writeFieldValue("Symbol (index)", i);

			OdDgLinePointResourceSymInfo symInfo = new OdDgLinePointResourceSymInfo();
			pRes.getSymbol(i, symInfo);
			pDumper.writeFieldValue("  symType", symInfo.getSymbolType());

			if (symInfo.getPointSymbolHandleId() == 0)
			{
				pDumper.writeFieldValue("  symEntryID", symInfo.getPointSymbolEntryId());
			}
			else
			{
				pDumper.writeFieldValueHex("  symID", symInfo.getPointSymbolHandleId());
			}

			pDumper.writeFieldValue("  strokeNo", symInfo.getSymbolStrokeNo());
			pDumper.writeFieldValue("  xOffset", symInfo.getOffset().x);
			pDumper.writeFieldValue("  yOffset", symInfo.getOffset().y);
			pDumper.writeFieldValue("  zAngle", symInfo.getRotationAngle());
			pDumper.writeFieldValue("  Symbol At Element Origin Flag", symInfo.getSymbolAtElementOriginFlag());
			pDumper.writeFieldValue("  Symbol At Element End Flag", symInfo.getSymbolAtElementEndFlag());
			pDumper.writeFieldValue("  Symbol At Each Vertex Flag", symInfo.getSymbolAtEachVertexFlag());
			pDumper.writeFieldValue("  Absolute Rotation Angle Flag", symInfo.getAbsoluteRotationAngleFlag());
			pDumper.writeFieldValue("  Partial Origin Beyond End Flag", symInfo.getPartialOriginBeyondEndFlag());
			pDumper.writeFieldValue("  No Partial Stroke Flag", symInfo.getNoPartialStrokesFlag());
			pDumper.writeFieldValue("  Mirror Symbol Flag", symInfo.getMirrorSymbolForReversedLinesFlag());
			pDumper.writeFieldValue("  Do Not Scale Element Flag", symInfo.getDoNotScaleElementFlag());
			pDumper.writeFieldValue("  Do Not Clip Element Flag", symInfo.getDoNotClipElementFlag());
			pDumper.writeFieldValue("  Symbol Position On Stroke", symInfo.getSymbolPosOnStroke());
			pDumper.writeFieldValue("  Use Symbol Color", symInfo.getUseSymbolColorFlag());
			pDumper.writeFieldValue("  Use Symbol Weight", symInfo.getUseSymbolWeightFlag());
		}
	}
}
class OdDgPointSymbolResourceDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgPointSymbolResource pRes = (OdDgPointSymbolResource)pObj;
		pDumper.writeFieldValue("Description", pRes.getDescription());
		pDumper.writeFieldValueHex("Symbol Handle Id", pRes.getDependedCellHeaderHandle());
		pDumper.writeFieldValue("rangeLow", pRes.getSymbolExtents().minPoint());
		pDumper.writeFieldValue("rangeHigh", pRes.getSymbolExtents().maxPoint());
		pDumper.writeFieldValue("offset", pRes.getSymbolOffset());
		pDumper.writeFieldValue("scale", pRes.getSymbolScale());
		pDumper.writeFieldValue("Symbol 3D Flag", pRes.getSymbol3DFlag());
	}
}
class OdDgLinearAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgLinearAssociation pAssoc = (OdDgLinearAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Vertex Number", pAssoc.getVertexNumber());
		pDumper.writeFieldValue("    Numerator", pAssoc.getNumerator());
		pDumper.writeFieldValue("    Divisor", pAssoc.getDivisor());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    N Vertices ", pAssoc.getNVertices());
	}
}
class OdDgIntersectAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgIntersectAssociation pAssoc = (OdDgIntersectAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Index", pAssoc.getIndex());
		pDumper.writeFieldValueHex("    Element 1 Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef 1 Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValueHex("    Element 2 Id", pAssoc.getElement2Id());
		pDumper.writeFieldValueHex("    XRef 2 Id", pAssoc.getRefAttachment2Id());
	}
}
class OdDgArcAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgArcAssociation pAssoc = (OdDgArcAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Key Point", pAssoc.getKeyPoint());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    Angle", pAssoc.getAngle());
	}
}
class OdDgMultilineAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgMultilineAssociation pAssoc = (OdDgMultilineAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Vertex Number", pAssoc.getVertexNumber());
		pDumper.writeFieldValue("    Line Number", pAssoc.getLineNumber());
		pDumper.writeFieldValue("    Joint Flag", pAssoc.getJointFlag());
		pDumper.writeFieldValue("    Project Flag", pAssoc.getProjectFlag());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    Offset", pAssoc.getOffset());
		pDumper.writeFieldValue("    N Vertices ", pAssoc.getNVertices());
	}
}
class OdDgBSplineCurveAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgBSplineCurveAssociation pAssoc = (OdDgBSplineCurveAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    U Param", pAssoc.getParam());
	}
}
class OdDgProjectionAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgProjectionAssociation pAssoc = (OdDgProjectionAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Vertex Number", pAssoc.getVertexNumber());
		pDumper.writeFieldValue("    N Vertices ", pAssoc.getNVertices());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    Fraction", pAssoc.getFraction());
	}
}
class OdDgOriginAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgOriginAssociation pAssoc = (OdDgOriginAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Origin Option", pAssoc.getTextOriginOption());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
	}
}
class OdDgIntersect2AssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgIntersect2Association pAssoc = (OdDgIntersect2Association)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    N Seg 1", pAssoc.getNSeg1());
		pDumper.writeFieldValue("    Index", pAssoc.getIndex());
		pDumper.writeFieldValue("    N Seg 2", pAssoc.getNSeg2());
		pDumper.writeFieldValue("    Segment 1", pAssoc.getSegment1());
		pDumper.writeFieldValue("    Segment 2", pAssoc.getSegment2());
		pDumper.writeFieldValueHex("    Element 1 Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef 1 Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValueHex("    Element 2 Id", pAssoc.getElement2Id());
		pDumper.writeFieldValueHex("    XRef 2 Id", pAssoc.getRefAttachment2Id());
	}
}
class OdDgMeshVertexAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgMeshVertexAssociation pAssoc = (OdDgMeshVertexAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Vertex Index", pAssoc.getVertexIndex());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    N Vertices", pAssoc.getNVertices());
	}
}

class OdDgMeshEdgeAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);

		OdDgMeshEdgeAssociation pAssoc = (OdDgMeshEdgeAssociation)pObj;

		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValue("    Edge Index", pAssoc.getEdgeIndex());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    U Param", pAssoc.getUParam());
		pDumper.writeFieldValue("    N Edges", pAssoc.getNEdges());
	}
}
class OdDgBSplineSurfaceAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);
		OdDgBSplineSurfaceAssociation pAssoc = (OdDgBSplineSurfaceAssociation)pObj;
		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
		pDumper.writeFieldValue("    U Param", pAssoc.getUParam());
		pDumper.writeFieldValue("    V Param", pAssoc.getVParam());
	}
}
class OdDgUnknownAssociationDumperPE : OdDgRxObjectDumperPE
{
	public override void dump(OdRxObject pObj, OdExDgnDumper pDumper)
	{
		base.dump(pObj, pDumper);
		OdDgUnknownAssociation pAssoc = (OdDgUnknownAssociation)pObj;
		pDumper.writeFieldValue("    Association Type", pAssoc.getType());
		pDumper.writeFieldValueHex("    Element Id", pAssoc.getElementId());
		pDumper.writeFieldValueHex("    XRef Id", pAssoc.getRefAttachmentId());
	}
}



