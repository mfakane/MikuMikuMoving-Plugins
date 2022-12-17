using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

/// <summary>
/// テキストによる情報表示を提供します。
/// </summary>
class NonInteractiveTextImage : NonInteractiveScreenImage2D
{
    static readonly string[] fontFamilies = new[]
    {
        @"ＭＳ ゴシック",
        @"Segoe UI Mono",
        @"Lucida Console",
    };
    
    readonly Scene scene;
    string? text;
    
    public string? Text
    {
        get => text;
        set
        {
            if (text == value) return;

            // ScreenImage_2D の Image を変更した場合、一度 Remove してから Add しなおさないと実際の表示に反映されない
            var needsReload = scene.ScreenObjects.Contains(this);
            
            if (needsReload) scene.ScreenObjects.Remove(this);
            
            Image = CreateGradientTextImage(text = value);
            
            if (needsReload) scene.ScreenObjects.Add(this);
        }
    }

    public NonInteractiveTextImage(Scene scene, string? text = null)
    {
        this.scene = scene;
        this.text = text;
        Image = CreateGradientTextImage(text);
    }

    [MustUseReturnValue]
    static Bitmap CreateGradientTextImage(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return new Bitmap(1, 1);
        
        var fonts = FontFamily.Families.Select(x => x.Name).ToArray();

        using var font = new Font(fontFamilies.First(fonts.Contains), 16, FontStyle.Bold);
        var measuredTextSize = TextRenderer.MeasureText(text, font);
        
        if (measuredTextSize.Width == 0 || measuredTextSize.Height == 0)
            return new Bitmap(1, 1);
        
        var bitmap = new Bitmap(Math.Max(measuredTextSize.Width, 1), Math.Max(measuredTextSize.Height, 1));
        
        using var g = Graphics.FromImage(bitmap);
        var y = 0;
        var rows = text!.Split('\n');
        var rowHeight = measuredTextSize.Height / rows.Length;
        var beginColor = Color.FromArgb(84, 115, 221);
        var endColor = Color.FromArgb(26, 43, 165);

        foreach (var row in rows)
        {
            using var textPath = new GraphicsPath();
            using var brushFillGradient = new LinearGradientBrush(new(0, y), new(0, y + rowHeight), beginColor, endColor);
            using var brushLightGradient = new LinearGradientBrush(new(0, y), new(0, y + rowHeight), Color.FromArgb(25, beginColor), Color.FromArgb(25, endColor));
            using var outlineGradient = new Pen(brushLightGradient, 3);
            using var outlineWhite = new Pen(Color.White, 3);
            var s = row.Trim('\r');

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            textPath.AddString(s, font.FontFamily, (int)font.Style, font.Size, new Point(0, y),
                StringFormat.GenericDefault);
            g.DrawPath(outlineWhite, textPath);
            g.DrawPath(outlineGradient, textPath);
            g.FillPath(brushFillGradient, textPath);
            y += rowHeight;
        }
        
        g.Flush();

        return bitmap;
    }

}