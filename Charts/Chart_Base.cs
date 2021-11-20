using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum NoteDirection
{
    up,down,left,right
}

[Serializable]
public class Note{

    public bool pressed=false;

    public NoteDirection Direction;
    public string sound;
    public int time;
    public float pitchOffset;
    public float noteSpeed;
    public Vector2 camOffset;

    public bool held;

    public Note(NoteDirection dir, int t, string sfx, int s = 1, bool hold = false, float offX = 0, float offY = 0,float pitch=0)
    {
        Direction = dir;
        time = t;
        sound = sfx;
        noteSpeed = s;
        held = hold;
        camOffset = new Vector2(offX, offY);
        pitchOffset = pitch;
    }

    public void Press()
    {
        pressed = true;
    }

}

[Serializable]
public class ChartSegment
{
    public int chartDuration;
    public Note[] notes;
    public Vector2 camPos;
}

namespace KingdomTerrahearts.Charts
{
    public abstract class Chart_Base
    {

        public float songTime=0;
        public int curSegment=0;
        public int accuracyRequired = 20;

        public string songName;
        public ChartSegment[] chart;

        public abstract void Effects();
        public abstract void SetDeffaults();
        public abstract void NoteEffects(NoteDirection direction,float speed);

        public virtual void CheckNote()
        {
            songTime++;

            if (songTime > chart[curSegment].chartDuration)
            {
                curSegment++;
                songTime = 0;
            }

            for(int i = 0; i < chart[curSegment].notes.Length; i++)
            {

            }
        }

    }
}
