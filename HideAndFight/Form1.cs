using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;

namespace HideAndFight
{
    public partial class Form1 : Form
    {
        public static int MAXHEIGHT = 20;
        public static int MAXWIDTH = 20;
        public static int MINHEIGHT = 0;
        public static int MINWIDTH = 0;
        public static int MOVEDISTANCE = 20;
        public static int iORIGINALATTACK = 10;
        public static int iBUFFATTACK = 30;
        public static int iBossLife = 100;
        public static int iORIGINALSPEED = 120;
        public static int iBUFFSPEED = 90;
        public static int iBUFFTIME = 5000;
        public static int iBossSpeedLow = 300;
        public static int iBossSpeedHeight = 100;

        private double dTimes = 0;
        private int iChrX = 12;
        private int iChrY = 12;
        private int iEnemyX = 12;
        private int iEnemyY = 19;
        private int iBulletX = 0;
        private int iBulletY = 0;
        private int iBulletVal = iORIGINALATTACK;
        private int iLife = iBossLife;
        private int iCoolDown = 0;
        private int iPlayerSpeed = iORIGINALSPEED;
        private String sFace = "";
        public bool bActLoop = true;
        public bool MoveUp = false;
        public bool MoveDown = false;
        public bool MoveLeft = false;
        public bool MoveRight = false;

        public delegate void MyInvoke(RadioButton rad);

        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
        public void Init()
        {
            label1.Visible = true;
            label2.Visible = true;
            label1.Text = "ᕙ('▿')ᕡ";
            label2.Text = "(ˋAˊ)";
            sFace = "S";
            iLife = iBossLife;
            Thread tRun = new Thread(EnemyStart);
            Thread time = new Thread(TimeCount);
            Thread tMove = new Thread(ChrMove);
            Thread tEvent = new Thread(RandEvent);
            time.Start();
            tRun.Start();
            tMove.Start();
            tEvent.Start();
        }

        /*------Enemy Move-------*/

        public void EnemyStart()
        {
            int iMove = 10;
            int iDash = 5;
            Random rnd = new Random();
            int iSelect = 0;
            while (bActLoop)
            {
                if (iChrX == iEnemyX && iChrY == iEnemyY)
                {
                    bActLoop = false;
                    Lose();
                }

                if (iLife <= 0)
                {
                    bActLoop = false;
                    MethodInvoker mi = new MethodInvoker(this.Win);
                    this.BeginInvoke(mi, null);
                    break;
                }
                if (iMove > iDash)
                    Thread.Sleep(iBossSpeedLow);
                else if (iMove <= iDash && iMove > 0)
                    Thread.Sleep(iBossSpeedHeight);
                else
                {
                    iMove = rnd.Next(7, 20);
                    iDash = rnd.Next(3, 7);
                }
                iMove--;

                int iSkill = rnd.Next(0, 40);
                if (iSkill == 1)
                {
                    MethodInvoker mSkill = new MethodInvoker(this.BossSkill);
                    this.BeginInvoke(mSkill, null);
                    iSkill = 10;
                }

                try
                {
                    iSelect = rnd.Next(0, 2);
                    if (iSelect == 0)
                    {
                        if (iEnemyX > iChrX)
                        {
                            MethodInvoker mi = new MethodInvoker(this.subNodeX);
                            this.BeginInvoke(mi, null);
                        }
                        else if (iEnemyX < iChrX)
                        {
                            MethodInvoker mi = new MethodInvoker(this.addNodeX);
                            this.BeginInvoke(mi, null);
                        }
                        else
                        {
                            if (iEnemyY > iChrY)
                            {
                                MethodInvoker mi = new MethodInvoker(this.subNodeY);
                                this.BeginInvoke(mi, null);
                            }
                            else if (iEnemyY < iChrY)
                            {
                                MethodInvoker mi = new MethodInvoker(this.addNodeY);
                                this.BeginInvoke(mi, null);
                            }
                        }
                    }
                    else if (iSelect == 1)
                    {
                        if (iEnemyY > iChrY)
                        {
                            MethodInvoker mi = new MethodInvoker(this.subNodeY);
                            this.BeginInvoke(mi, null);
                        }
                        else if (iEnemyY < iChrY)
                        {
                            MethodInvoker mi = new MethodInvoker(this.addNodeY);
                            this.BeginInvoke(mi, null);
                        }
                        else
                        {
                            if (iEnemyX > iChrX)
                            {
                                MethodInvoker mi = new MethodInvoker(this.subNodeX);
                                this.BeginInvoke(mi, null);
                            }
                            else if (iEnemyX < iChrX)
                            {
                                MethodInvoker mi = new MethodInvoker(this.addNodeX);
                                this.BeginInvoke(mi, null);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    bActLoop = false;
                }
            }
        }

        private void addNodeX()
        {
            label2.Location = new Point(label2.Location.X + MOVEDISTANCE, label2.Location.Y);
            iEnemyX++;
        }
        private void subNodeX()
        {
            label2.Location = new Point(label2.Location.X - MOVEDISTANCE, label2.Location.Y);
            iEnemyX--;
        }
        private void addNodeY()
        {
            label2.Location = new Point(label2.Location.X, label2.Location.Y - MOVEDISTANCE);
            iEnemyY++;
        }
        private void subNodeY()
        {
            label2.Location = new Point(label2.Location.X, label2.Location.Y + MOVEDISTANCE);
            iEnemyY--;
        }

        public void BossSkill()
        {
            Random rnd = new Random();
            int moveX = rnd.Next(-10, 10);
            int moveY = rnd.Next(-10, 10);
            iEnemyX += moveX;
            iEnemyY += moveY;
            label2.Location = new Point(label2.Location.X + MOVEDISTANCE * moveX, label2.Location.Y - MOVEDISTANCE * moveY);
        }

        /*-------Ending------*/
        public void Lose()
        {
            MethodInvoker mEnd = new MethodInvoker(this.End);
            try
            {
                MessageBox.Show("Game Over    " + dTimes.ToString("0.0") + "s\n(" + iLife + "/" + iBossLife + ")");
                chrStop();
                this.BeginInvoke(mEnd, null);
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.ToString());
            }
        }

        public void Win()
        {
            label2.Text = "(X A X)";
            chrStop();
            MessageBox.Show("Win  " + dTimes.ToString("0.0") + "s");
            Thread tWin = new Thread(WinStatus);
            tWin.Start();
        }

        private void WinStatus()
        {
            MethodInvoker mW1 = new MethodInvoker(this.chrWin1);
            MethodInvoker mW2 = new MethodInvoker(this.chrWin2);
            while (true)
            {
                try
                {
                    this.BeginInvoke(mW1, null);
                    Thread.Sleep(200);
                    this.BeginInvoke(mW2, null);
                    Thread.Sleep(200);
                }
                catch(Exception ec)
                {
                    String error = ec.ToString();
                    break;
                }
            }
        }

        private void chrWin1()
        {
            label1.Text = "ᕙ(ᐖ)ᕡ";
        }
        private void chrWin2()
        {
            label1.Text = "ᕕ(ᐛ)ᕗ";
        }

        private void End()
        {
            this.Close();
        }

        //timer
        public void TimeCount()
        {
            while (bActLoop)
            {
                dTimes += 0.1;
                Thread.Sleep(100);
            }
        }

        /*------Character Move--------*/

        public void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!bActLoop)
                return;
            if ((e.KeyChar == 'W' || e.KeyChar == 'w') && iChrY <= MAXHEIGHT)
            {
                chrStop();
                MoveUp = true;
            }
            else if ((e.KeyChar == 'S' || e.KeyChar == 's') && iChrY >= MINHEIGHT)
            {
                chrStop();
                MoveDown = true;
            }
            else if ((e.KeyChar == 'A' || e.KeyChar == 'a') && iChrX >= MINWIDTH)
            {
                chrStop();
                MoveLeft = true;
            }
            else if ((e.KeyChar == 'D' || e.KeyChar == 'd') && iChrX <= MAXWIDTH)
            {
                chrStop();
                MoveRight = true;
            }
            else if (e.KeyChar == ' ' && iCoolDown == 0)
            {
                if (iCoolDown == 0)
                {
                    Thread tBullet = new Thread(BulletShut);
                    tBullet.Start();
                    iCoolDown = 1;
                    Thread tCool = new Thread(CoolTime);
                    tCool.Start();
                }
            }
            else if (e.KeyChar == '\x1B')
            {
                MethodInvoker mEnd = new MethodInvoker(this.End);
                this.BeginInvoke(mEnd, null);
            }
        }

        private void ChrMove()
        {
            try
            {
                while (bActLoop)
                {
                    while (MoveUp && iChrY <= MAXHEIGHT)
                    {
                        MethodInvoker mU = new MethodInvoker(this.chrUp);
                        this.BeginInvoke(mU, null);
                        Thread.Sleep(iPlayerSpeed);
                    }
                    while (MoveDown && iChrY >= MINHEIGHT)
                    {
                        MethodInvoker mD = new MethodInvoker(this.chrDown);
                        this.BeginInvoke(mD, null);
                        Thread.Sleep(iPlayerSpeed);
                    }
                    while (MoveLeft && iChrX >= MINWIDTH)
                    {
                        MethodInvoker mL = new MethodInvoker(this.chrLeft);
                        this.BeginInvoke(mL, null);
                        Thread.Sleep(iPlayerSpeed);
                    }
                    while (MoveRight && iChrX <= MAXWIDTH)
                    {
                        MethodInvoker mR = new MethodInvoker(this.chrRight);
                        this.BeginInvoke(mR, null);
                        Thread.Sleep(iPlayerSpeed);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                bActLoop = false;
            }
        }

        private void chrUp()
        {
            label1.Location = new Point(label1.Location.X, label1.Location.Y - MOVEDISTANCE);
            label1.Text = "ᕕ(" + new String(' ', 3) + ")ᕗ";
            sFace = "W";
            iChrY++;
        }
        private void chrDown()
        {
            label1.Location = new Point(label1.Location.X, label1.Location.Y + MOVEDISTANCE);
            label1.Text = "ᕙ('▿')ᕡ";
            sFace = "S";
            iChrY--;
        }
        private void chrLeft()
        {
            label1.Location = new Point(label1.Location.X - MOVEDISTANCE, label1.Location.Y);
            label1.Text = "ᕙ(ᐖ)ᕡ";
            sFace = "A";
            iChrX--;
        }
        private void chrRight()
        {
            label1.Location = new Point(label1.Location.X + MOVEDISTANCE, label1.Location.Y);
            label1.Text = "ᕕ(ᐛ)ᕗ";
            sFace = "D";
            iChrX++;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "W")
                MoveUp = false;
            else if (e.KeyCode.ToString() == "A")
                MoveLeft = false;
            else if (e.KeyCode.ToString() == "S")
                MoveDown = false;
            else if (e.KeyCode.ToString() == "D")
                MoveRight = false;
        }

        public void chrStop()
        {
            MoveUp = false;
            MoveDown = false;
            MoveLeft = false;
            MoveRight = false;
        }

        /*---------Attack--------*/

        public void BulletShut()
        {
            int iFace = 0;

            try
            {
                MethodInvoker mInit = new MethodInvoker(this.locBullet);
                this.BeginInvoke(mInit, null);


                if (sFace == "W")
                {
                    iFace = 1;
                }
                else if (sFace == "D")
                {
                    iFace = 2;
                }
                else if (sFace == "S")
                {
                    iFace = 3;
                }
                else if (sFace == "A")
                {
                    iFace = 4;
                }
                for (int i = 5; i >= 0; i--)
                {
                    if (iBulletX == iEnemyX && iBulletY == iEnemyY)
                    {
                        iLife -= iBulletVal;
                        break;
                    }
                    else
                    {
                        if (iFace == 1)
                        {
                            if (iBulletX == iEnemyX && (iBulletY - 1) == iEnemyY || (iBulletY - 2) == iEnemyY)
                            {
                                iLife -= iBulletVal;
                                break;
                            }
                            MethodInvoker mi = new MethodInvoker(this.addBulletY);
                            this.BeginInvoke(mi, null);
                        }
                        else if (iFace == 3)
                        {
                            if (iBulletX == iEnemyX && (iBulletY + 1) == iEnemyY || (iBulletY + 2) == iEnemyY)
                            {
                                iLife -= iBulletVal;
                                break;
                            }
                            MethodInvoker mi = new MethodInvoker(this.subBulletY);
                            this.BeginInvoke(mi, null);
                        }
                        else if (iFace == 4)
                        {
                            if ((iBulletX + 1) == iEnemyX || (iBulletX + 2) == iEnemyX && iBulletY == iEnemyY)
                            {
                                iLife -= iBulletVal;
                                break;
                            }
                            MethodInvoker mi = new MethodInvoker(this.subBulletX);
                            this.BeginInvoke(mi, null);
                        }
                        else if (iFace == 2)
                        {
                            if ((iBulletX - 1) == iEnemyX || (iBulletX - 2) == iEnemyX && iBulletY == iEnemyY)
                            {
                                iLife -= iBulletVal;
                                break;
                            }
                            MethodInvoker mi = new MethodInvoker(this.addBulletX);
                            this.BeginInvoke(mi, null);
                        }
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                MethodInvoker me = new MethodInvoker(this.endBullet);
                this.BeginInvoke(me, null);
            }
        }

        private void locBullet()
        {
            label3.Location = label1.Location;
            iBulletX = iChrX;
            iBulletY = iChrY;
        }
        private void addBulletY()
        {
            label3.Text = new String(' ', 1) + "^";
            label3.Location = new Point(label3.Location.X, label3.Location.Y - MOVEDISTANCE);
            iBulletY++;
        }
        private void subBulletY()
        {
            label3.Text = new String(' ', 1) + "v";
            label3.Location = new Point(label3.Location.X, label3.Location.Y + MOVEDISTANCE);
            iBulletY--;
        }
        private void addBulletX()
        {
            label3.Text = ">";
            label3.Location = new Point(label3.Location.X + MOVEDISTANCE, label3.Location.Y);
            iBulletX++;
        }
        private void subBulletX()
        {
            label3.Text = "<";
            label3.Location = new Point(label3.Location.X - MOVEDISTANCE, label3.Location.Y);
            iBulletX--;
        }
        private void endBullet()
        {
            label3.Text = "";
        }
        private void CoolTime()
        {
            try
            {
                for (int i = 0; i <= 4; i++)
                {
                    MethodInvoker mLoad = new MethodInvoker(this.BarLoad);
                    this.BeginInvoke(mLoad, null);
                }
                Thread.Sleep(750);
                iCoolDown = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void BarLoad()
        {
            if (coolBar.Value >= 100)
                coolBar.Value = 0;
            else
                coolBar.Value += 25;
        }

        //Chracter Buff
        private void Buff()
        {
            MethodInvoker mBuff = new MethodInvoker(this.BuffStatus);
            try
            {
                iBulletVal = iBUFFATTACK;
                iPlayerSpeed = iBUFFSPEED;
                this.BeginInvoke(mBuff, null);
                Thread.Sleep(iBUFFTIME);
                iBulletVal = iORIGINALATTACK;
                iPlayerSpeed = iORIGINALSPEED;
                this.BeginInvoke(mBuff, null);
            }
            catch (Exception ec)
            {
                MessageBox.Show("Error" + ec.ToString());
            }
        }

        private void BuffStatus()
        {
            if (label4.Visible)
                label4.Visible = false;
            else
                label4.Visible = true;
        }

        //Random Item
        private void RandEvent()
        {
            int iNum = 0;
            Random rnd = new Random();
            int iItemX, iItemY;
            MyInvoke mAdd = new MyInvoke(this.AddItem);
            MyInvoke mRem = new MyInvoke(this.RemItem);
            Thread tBuff = new Thread(Buff);
            int iLoading = 0;
            try
            {
                while (iLoading != 1)
                {
                    iLoading = rnd.Next(0, 80);
                    Thread.Sleep(100);
                }
                iLoading = 0;

                while (bActLoop)
                {
                    RadioButton radItem = new RadioButton();
                    radItem.Name = "SpawnItem" + iNum;
                    radItem.Text = "";
                    iItemX = rnd.Next(MINWIDTH, MAXWIDTH);
                    iItemY = rnd.Next(MINHEIGHT, MAXHEIGHT);
                    radItem.Location = new Point(iItemX * MOVEDISTANCE + 30, 420 - iItemY * MOVEDISTANCE);
                    radItem.Enabled = false;

                    this.BeginInvoke(mAdd, radItem);

                    while (true)
                    {
                        radItem.Text = "";
                        if (iItemX == iChrX && iItemY == iChrY)
                        {
                            tBuff = new Thread(Buff);
                            tBuff.Start();
                            this.BeginInvoke(mRem, radItem);
                            break;
                        }
                    }
                    while (iLoading != 1)
                    {
                        iLoading = rnd.Next(0, 80);
                        Thread.Sleep(100);
                    }
                    iLoading = 0;
                    Thread.Sleep(5000);
                    tBuff.Interrupt();
                }
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.ToString());
            }
        }

        private void AddItem(RadioButton radItem)
        {
            this.Controls.Add(radItem);
        }
        private void RemItem(RadioButton radItem)
        {
            this.Controls.Remove(radItem);
        }
    }
}
