using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class HUD : Observer
{
    private int PlayerNumber;

    private Player Player;

    private Color BackColor;

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

    public HUD(int playerNumber, Player p, Color backColor)
    {

        Player = p;
        PlayerNumber = playerNumber;
        BackColor = backColor;
        Messages = new MessageQueue();
    }

    public void Update()
    {
        Messages.Update();
    }

    public void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        Utils.drawRect(sb, 0, 0, 200, 20, BackColor);
        Utils.drawRect(sb, 0, 0, 70, 150, BackColor);

        //print message in message zone.
        f.Draw(sb, Messages.CurrentMessage.RevealedText(), 0, 11, Color.White);
        f.Draw(sb, Messages.LastMessage.RevealedText(), 0, 1, Color.White);

        //Print HP Bar.
        Utils.drawRect(sb, 1, 23, 67, 13, Color.White);
        if (Player.HP > 0)
        {
            Utils.drawRect(sb, 2, 24, (int)(65 * ((double)Player.HP / Player.MaxHP())), 11, Color.Red);
        }

        //Print HP. Max is 4 * 100 = 400.
        f.Draw(sb, Player.HP +"", 4, 26, Color.Black,1,true);
    }


    public override void Observe(string eventMessage)
    {
        Messages.Enqueue(eventMessage);
    }
}