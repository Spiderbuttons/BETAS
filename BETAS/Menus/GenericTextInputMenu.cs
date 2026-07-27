using System;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.TokenizableStrings;

namespace BETAS.Menus;

public sealed class GenericTextInputMenu : IClickableMenu
{
    private const int DONE_INPUT_BUTTON = 102;
    private const int TEXT_BOX_BUTTON = 104;
    
    public delegate void afterInputBehaviour(string text);
    public afterInputBehaviour? afterInput;
    
    public ClickableTextureComponent doneInputButton;

    public int minLength = 0;

    public TextBox textBox;
    public ClickableComponent textBoxCC;
    
    public string? title;
    public string? defaultText;
    public string modDataKey;

    public IClickableMenu? prevMenu;

    public GenericTextInputMenu(afterInputBehaviour afterInput, string? title, string? defaultText, string modDataKey,
        int minChars = -1, int maxChars = -1, bool numbersOnly = false, IClickableMenu? prevMenu = null)
    {
        this.afterInput = afterInput;
        this.title = title;
        this.defaultText = defaultText;
        this.modDataKey = modDataKey;
        this.prevMenu = prevMenu;
        this.minLength = minChars;
        
        xPositionOnScreen = 0;
        yPositionOnScreen = 0;
        width = Game1.uiViewport.Width;
        height = Game1.uiViewport.Height;
        
        textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor)
        {
            X = Game1.uiViewport.Width / 2 - 256 - 32 - spaceToClearSideBorder / 2,
            Y = Game1.uiViewport.Height / 2,
            Width = 512,
            Height = 192,
            limitWidth = false,
            textLimit = maxChars < 1 ? 256 : maxChars,
            numbersOnly = numbersOnly
        };
        
        textBox.OnEnterPressed += textBoxEnter;
        Game1.keyboardDispatcher.Subscriber = textBox;
        textBox.Text = defaultText ?? string.Empty;
        textBox.Selected = true;
        
        doneInputButton = new ClickableTextureComponent(new Rectangle(textBox.X + textBox.Width + 32 + 4, Game1.uiViewport.Height / 2 - 6, 64, 64), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f)
        {
            myID = DONE_INPUT_BUTTON,
            leftNeighborID = TEXT_BOX_BUTTON
        };
        
        textBoxCC = new ClickableComponent(new Rectangle(textBox.X, textBox.Y, 192, 48), "")
        {
            myID = TEXT_BOX_BUTTON,
            rightNeighborID = DONE_INPUT_BUTTON
        };
        
        if (Game1.options.SnappyMenus)
        {
            populateClickableComponentList();
            snapToDefaultClickableComponent();
        }
    }
    
    public override void draw(SpriteBatch b)
    {
        base.draw(b);
        if (!Game1.options.showClearBackgrounds)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
        }
        if (title is not null) SpriteText.drawStringWithScrollCenteredAt(b, title, Game1.uiViewport.Width / 2, Game1.uiViewport.Height / 2 - 128, title);
        textBox.Draw(b);
        Color buttonColour = textBox.Text.Length >= minLength ? Color.White : Color.White * 0.4f;
        doneInputButton.draw(b, buttonColour, 1f);
        drawMouse(b);
    }
    
    public override void performHoverAction(int x, int y)
    {
        base.performHoverAction(x, y);

        if (textBox.Text.Length < minLength) return;
        if (doneInputButton.containsPoint(x, y))
        {
            doneInputButton.scale = Math.Min(1.1f, doneInputButton.scale + 0.05f);
        }
        else
        {
            doneInputButton.scale = Math.Max(1f, doneInputButton.scale - 0.05f);
        }
    }
    
    public override void receiveGamePadButton(Buttons button)
    {
        base.receiveGamePadButton(button);
        if (!textBox.Selected) return;

        textBox.Selected = button switch
        {
            Buttons.DPadUp or Buttons.DPadDown or Buttons.DPadLeft or Buttons.DPadRight or Buttons.LeftThumbstickLeft
                or Buttons.LeftThumbstickUp or Buttons.LeftThumbstickDown or Buttons.LeftThumbstickRight => false,
            _ => textBox.Selected
        };
    }
    
    public override void receiveKeyPress(Keys key)
    {
        if (!textBox.Selected && !Game1.options.doesInputListContain(Game1.options.menuButton, key))
        {
            base.receiveKeyPress(key);
        }
    }
    
    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
        base.receiveLeftClick(x, y, playSound);
        textBox.Update();
        if (doneInputButton.containsPoint(x, y) && textBox.Text.Length >= minLength)
        {
            textBoxEnter(textBox);
            Game1.playSound("smallSelect");
        }
    }
    
    public override void snapToDefaultClickableComponent()
    {
        currentlySnappedComponent = getComponentWithID(TEXT_BOX_BUTTON);
        snapCursorToCurrentSnappedComponent();
    }
    
    public void textBoxEnter(TextBox sender)
    {
        if (Game1.textEntry is not null || sender.Text.Length < minLength) return;
        
        if (afterInput != null)
        {
            afterInput(sender.Text);
            textBox.Selected = false;
        }
        Game1.playSound("smallSelect");

        if (prevMenu is DialogueBox dBox)
        {
            parseDialogueBox(dBox);
            dBox.transitioning = true;
            dBox.transitioningBigger = true;
            dBox.transitionX = -1;
            dBox.transitionY = 0;
            dBox.transitionHeight = 0;
            dBox.transitionWidth = 0;
            dBox.transitionInitialized = false;
            Game1.activeClickableMenu = dBox;
        } else if (Game1.CurrentEvent is { } currEvent)
        {
            // TODO: This will need attention in 1.7 since it will be changing when event commands get their tokens parsed.
            Game1.activeClickableMenu = null;
            for (int i = currEvent.eventCommands.Length - 1; i >= 0; i--)
            {
                currEvent.eventCommands[i] = TokenParser.ParseText(currEvent.eventCommands[i]);
            }

            currEvent.CurrentCommand++;
        }
        else Game1.activeClickableMenu = null;
    }

    public void parseDialogueBox(DialogueBox? dBox)
    {
        if (dBox is null) return;
        
        dBox.characterDialoguesBrokenUp = [];
                
        for (var index = 0; index < dBox.dialogues.Count; index++)
        {
            var dLine = dBox.dialogues[index];
            dBox.dialogues[index] = TokenParser.ParseText(dLine);
        }
                
        for (var index = 0; index < dBox.characterDialogue.dialogues.Count; index++)
        {
            var cLine = dBox.characterDialogue.dialogues[index];
            if (cLine.HasText)
            {
                Log.Alert(cLine.Text);
                Log.Debug(TokenParser.ParseText(cLine.Text));
                dBox.characterDialogue.dialogues[index] = new DialogueLine(TokenParser.ParseText(cLine.Text), cLine.SideEffects);
            }
        }
    }
}