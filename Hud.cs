using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class HUD : Observer
{
    private int PlayerNumber;

    private PrimaryWeapon PrimaryWeaponDisplay;

    private Player Player;

    //A Line of text that is revealed over time. Typical RPG fare.
    private class TextLine
    {
        private string Message;
        private int HiddenCharacters;

        public TextLine(string message)
        {
            Message = message;
            HiddenCharacters = Message.Length;
        }

        public bool Done()
        {
            return HiddenCharacters == 0;
        }

        public void Reveal()
        {
            if(HiddenCharacters != 0)
                HiddenCharacters--;
        }

        public string RevealedText()
        {
            return Message.Substring(0, Message.Length - HiddenCharacters);
        }
    }

    private class MessageQueue
    {
        private LinkedList<TextLine> Queue;
        public TextLine LastMessage { get; private set; }
        public TextLine CurrentMessage { get; private set; }

        public MessageQueue()
        {
            Queue = new LinkedList<TextLine>();
            LastMessage = new TextLine("");
            CurrentMessage = new TextLine("");
        }

        public void Enqueue(string msg)
        {
            Queue.AddLast(new TextLine(msg));
        }

        public void Update()
        {
            //always expand the current message.
            CurrentMessage.Reveal();
            //Bring up a new message if we can
            if (CurrentMessage.Done() && Queue.Count > 0)
            {
                LastMessage = CurrentMessage;
                CurrentMessage = Queue.First.Value;
                Queue.RemoveFirst();
            }
        }            
      }

    private MessageQueue Messages;

    public HUD(int playerNumber, Player p)
    {

        Player = p;
        PlayerNumber = playerNumber;
        Messages = new MessageQueue();
    }

    public void Update()
    {
        PrimaryWeaponDisplay = null;
        Messages.Update();
    }

    public void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        Utils.DrawRect(sb, 0, 0, 200, 20, Player.DarkColor);
        Utils.DrawRect(sb, 0, 0, 70, 150, Player.DarkColor);

        //print message in message zone.
        f.Draw(sb, Messages.CurrentMessage.RevealedText(), 2, 11, Color.White);
        f.Draw(sb, Messages.LastMessage.RevealedText(), 2, 1, Color.White);

        //Print HP Bar.
        Utils.DrawRect(sb, 1, 23, 67, 13, Color.White);
        if (Player.HP > 0)
        {
            Utils.DrawRect(sb, 2, 24, (int)(65 * ((double)Player.HP / Player.MaxHP())), 11, Color.Red);
        }

        string HPmsg = Player.HP +"";
        //center it up.
        //ignore space of last letter.
        f.Draw(sb, HPmsg, 35 - ((f.GetWidth(HPmsg, true) - 2) / 2), 26,Color.White,1,true);

        PrimaryWeapon primaryDisplay = PrimaryWeaponDisplay == null ? Player.PrimaryWeapon : PrimaryWeaponDisplay;

        if (PrimaryWeaponDisplay != null)
        {
            Utils.DrawRect(sb, 0, 37, 70, 59, Player.MainColor);

        }

        if (primaryDisplay != null)
        {
            f.Draw(sb, primaryDisplay.Name, 2, 38, Color.White, 1);

            f.Draw(sb, "Pow:  " + primaryDisplay.Power(Player), 2, 48, Color.White);
            f.Draw(sb, "Acc:  " + primaryDisplay.Accuracy(Player), 2, 58, Color.White);
            //Special ability here if applicable.
            //TODO: CRIT, +HP, -HP, STUN,
            f.Draw(sb, primaryDisplay.SpecialMessage(), 2, 68, Color.White);
            f.Draw(sb, primaryDisplay.WarningMessage(Player), 2, 78, Color.Gray);
        } else
        {
            f.Draw(sb, "Punch", 2, 38, Color.White, 1);

            f.Draw(sb, "Pow:  " + Player.PunchPower(), 2, 48, Color.White);
            f.Draw(sb, "Acc:  " + (Player).PunchAccuracy(), 2, 58, Color.White);

        }

        f.Draw(sb, "9 Darts", 2, 98, Color.White);
        f.Draw(sb, "Pow:  99", 2, 108, Color.White);
        f.Draw(sb, "Range 99", 2, 118, Color.White);

        Utils.DrawRect(sb, 24, 129, 12, 12, Color.Yellow);
        for (int i = 3; i < 62; i = i + 11)
        {
            Utils.DrawRect(sb, i, 130, 10, 10, Color.Gray);
        }

        f.Draw(sb, ".", 1+4, 131, Color.White);
        f.Draw(sb, "=", 1+15, 131, Color.Red);
        f.Draw(sb, ",", 1+26, 131, Color.Green);
        f.Draw(sb, "Y", 1+37, 131, Color.Brown);
 
    }


    public override void Observe(string eventMessage)
    {
        Messages.Enqueue(eventMessage);
    }

    public override void Observe(string eventMessage, object thing)
    {
        PrimaryWeaponDisplay = (PrimaryWeapon)thing;
    }
}