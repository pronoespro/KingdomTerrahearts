using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomTerrahearts.Charts
{
    public class Test_Chart : Chart_Base
    {

        public override void Effects()
        {

        }

        public override void NoteEffects(NoteDirection direction, float speed)
        {

        }

        public override void SetDeffaults()
        {
            songName = "Testing";

            chart = new ChartSegment[2];

            for (int i = 0; i < chart.Length; i++)
            {
                chart[i].notes = new Note[]
                {
                    new Note(NoteDirection.up,25,"Sounds/keybladeBlocking"),
                    new Note(NoteDirection.down,50,"Sounds/keybladeBlocking"),
                    new Note(NoteDirection.down,100,"Sounds/keybladeBlocking"),
                    new Note(NoteDirection.left,150,"Sounds/keybladeBlocking"),
                    new Note(NoteDirection.right,150,"Sounds/keybladeBlocking")
            };
            }
        }
    }
}
