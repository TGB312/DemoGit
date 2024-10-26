using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace TranGiaBao
{
    public partial class Form27wp : Form
    {
        private PictureBox pbBasket = new PictureBox();
        private PictureBox pbEgg = new PictureBox();
        private PictureBox pbGoldEgg = new PictureBox();
        private PictureBox pbChicken = new PictureBox();
        private Timer tmEgg = new Timer();
        private Timer tmChicken = new Timer();
        private Timer tmScoreEffect = new Timer();
        private Timer tmBasketMovement = new Timer();
        private Label lblScoreEffect = new Label();
        private Label lblTime = new Label();
        private Random random = new Random();
        private float[] heartOpacities = new float[3] { 1.0f, 1.0f, 1.0f }; // Độ mờ của mỗi trái tim

        private int elapsedTime = 0;
        private int score = 0;
        private int xBasket = 300;
        private int yBasket = 550;
        private int xDeltaBasket = 15;
        private int xChicken = 300;
        private int yChicken = 10;
        private int xDeltaChicken = 8;
        private int xEgg;
        private int yEgg = 10;
        private int yDeltaEgg = 7;
        private int xGoldEgg;
        private int yGoldEgg = 10;
        private bool isGoldEggFalling = false;
        private bool isNormalEggFalling = false;
        private bool isGameOver = false;
        private bool moveRight = false;
        private bool moveLeft = false;
        private int level = 1;
        private int currentLevel = 1;
        private int nextLevelScore = 15; // Điểm cần để tăng cấp tiếp theo.
        private int lives = 3;  // Khởi tạo 3 mạng.
        private Label lblLives = new Label();  // Label hiển thị số mạng.
        private PictureBox[] hearts = new PictureBox[3];  // Mảng chứa 3 trái tim.
        private SoundPlayer soundPlayer = new SoundPlayer("../../Sound/nhattrung.wav");
        private SoundPlayer gameOverSound = new SoundPlayer("../../Sound/gameover.wav");
        private SoundPlayer goldEggSound = new SoundPlayer("../../Sound/gold.wav");
        private SoundPlayer backgroundMusic = new SoundPlayer("../../Sound/nhac.wav");


        public Form27wp()
        {

            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            InitializeLabels();
        }

        private void Form27wp_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("../../Images/background.jpeg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            PlayBackgroundMusic();
            ConfigureTimers();
            InitializeGameObjects();
            InitializeHearts();
            ResetGame();
        }
        private void InitializeHearts()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = new PictureBox();
                hearts[i].Size = new Size(40, 40);  
                hearts[i].SizeMode = PictureBoxSizeMode.StretchImage;
                hearts[i].Image = Image.FromFile("../../Images/tim.png");  
                hearts[i].BackColor = Color.Transparent;

                hearts[i].Location = new Point(this.ClientSize.Width - (i + 1) * 50, 10);
                this.Controls.Add(hearts[i]);
            }
        }
        private void InitializeLabels()
        {
            // Label thời gian
            lblTime.AutoSize = true;
            lblTime.BackColor = Color.Transparent;
            lblTime.ForeColor = Color.DarkBlue;  // Màu xanh đậm
            lblTime.Font = new Font("Arial", 14, FontStyle.Bold | FontStyle.Italic);
            lblTime.Location = new Point(15, 10); // Cách viền trên và trái form 15px
            this.Controls.Add(lblTime);

            // Label điểm
            lblScore.AutoSize = true;
            lblScore.BackColor = Color.Transparent;
            lblScore.ForeColor = Color.DarkRed;  // Màu đỏ đậm
            lblScore.Font = new Font("Arial", 14, FontStyle.Bold | FontStyle.Italic);
            lblScore.Location = new Point(lblTime.Right + 20, lblTime.Top); // Nằm bên phải label thời gian
            this.Controls.Add(lblScore);

            // Label hiệu ứng điểm (+1, +2)
            lblScoreEffect.AutoSize = true;
            lblScoreEffect.BackColor = Color.Transparent;
            lblScoreEffect.ForeColor = Color.DarkGreen;  // Màu xanh lá đậm
            lblScoreEffect.Font = new Font("Arial", 18, FontStyle.Bold);
            lblScoreEffect.Visible = false;
            this.Controls.Add(lblScoreEffect);

            Label lblLevel = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.Purple,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(lblScore.Right + 20, lblTime.Top),
                Text = $"Level: {level}"
            };
            this.Controls.Add(lblLevel);
            // Label mạng
            lblLives.AutoSize = true;
            lblLives.BackColor = Color.Transparent;
            lblLives.ForeColor = Color.Purple;
            lblLives.Font = new Font("Arial", 14, FontStyle.Bold);
            lblLives.Location = new Point(lblScore.Right + 20, lblScore.Top);
            lblLives.Text = $"Mạng: {lives}";
            this.Controls.Add(lblLives);
            

        }

        private void PlayBackgroundMusic()
        {
            try
            {
                backgroundMusic.PlayLooping();  // Phát nhạc lặp
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể phát nhạc nền: {ex.Message}");
            }
        }
        private void StopBackgroundMusic()
        {
            backgroundMusic.Stop();  // Dừng nhạc
        }
        private void ConfigureTimers()
        {
            tmEgg.Interval = 8;
            tmEgg.Tick += tmEgg_Tick;
            tmEgg.Start();

            tmChicken.Interval = 25;
            tmChicken.Tick += tmChicken_Tick;
            tmChicken.Start();

            tmScoreEffect.Interval = 500;
            tmScoreEffect.Tick += TmScoreEffect_Tick;

            tmBasketMovement.Interval = 30;
            tmBasketMovement.Tick += TmBasketMovement_Tick;
            tmBasketMovement.Start();
        }

        private void InitializeGameObjects()
        {
            InitializeBasket();
            InitializeEggs();
            InitializeChicken();

            this.KeyDown += Form27wp_KeyDown;
            this.KeyUp += Form27wp_KeyUp;

            Timer tmTime = new Timer();
            tmTime.Interval = 1000;
            tmTime.Tick += TmTime_Tick;
            tmTime.Start();
        }

        private void InitializeBasket()
        {
            pbBasket.SizeMode = PictureBoxSizeMode.StretchImage;
            pbBasket.Size = new Size(80, 80);
            pbBasket.Location = new Point(xBasket, yBasket);
            pbBasket.BackColor = Color.Transparent;
            pbBasket.Image = Image.FromFile("../../Images/ro2.png");
            this.Controls.Add(pbBasket);
        }

        private void InitializeEggs()
        {
            pbEgg.SizeMode = PictureBoxSizeMode.StretchImage;
            pbEgg.Size = new Size(30, 50);
            ResetEgg();
            pbEgg.BackColor = Color.Transparent;
            pbEgg.Image = Image.FromFile("../../Images/trungga.png");
            this.Controls.Add(pbEgg);

            pbGoldEgg.SizeMode = PictureBoxSizeMode.StretchImage;
            pbGoldEgg.Size = new Size(35, 35);
            pbGoldEgg.BackColor = Color.Transparent;
            pbGoldEgg.Image = Image.FromFile("../../Images/gold.png");
            pbGoldEgg.Visible = false;
            this.Controls.Add(pbGoldEgg);
        }

        private void InitializeChicken()
        {
            pbChicken.SizeMode = PictureBoxSizeMode.StretchImage;
            pbChicken.Size = new Size(100, 100);
            pbChicken.Location = new Point(xChicken, yChicken);
            pbChicken.BackColor = Color.Transparent;
            pbChicken.Image = Image.FromFile("../../Images/gagag.png");
            this.Controls.Add(pbChicken);
        }

        private void TmTime_Tick(object sender, EventArgs e)
        {
            elapsedTime++;
            lblTime.Text = $"Thời gian: {elapsedTime}s";
            if (elapsedTime % 10 == 0)
            {
                lblTime.ForeColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
            // Cập nhật vị trí cho label điểm
            lblScore.Location = new Point(lblTime.Right + 20, lblTime.Top);
        }

        private void HandleNormalEgg()
        {
            if (isNormalEggFalling)
            {
                yEgg += yDeltaEgg;
                pbEgg.Location = new Point(xEgg, yEgg);
                pbEgg.Visible = true;

                if (pbBasket.Bounds.IntersectsWith(pbEgg.Bounds) && yEgg + pbEgg.Height >= yBasket)
                {
                    score += 1;  // Cộng 1 điểm cho trứng thường.
                    PlayCatchSound();
                    UpdateScoreLabel();  // Cập nhật hiển thị điểm.
                    ShowScoreEffect("+1");
                    ResetEgg();
                }
                else if (yEgg > this.ClientSize.Height)  // Trứng rơi xuống đất.
                {
                    lives--;  // Giảm mạng.
                    UpdateHearts();  // Cập nhật trái tim.

                    if (lives == 0)
                    {
                        isGameOver = true;
                        ShowGameOverMessage();  // Kết thúc trò chơi nếu hết mạng.
                    }

                    ResetEgg();
                }
            }
        }
        private void UpdateHearts()
        {
            if (lives >= 0 && lives < hearts.Length)
            {
                // Giảm độ mờ của trái tim tương ứng
                heartOpacities[lives] -= 1f; // Giảm độ mờ mỗi khi mất mạng
                if (heartOpacities[lives] < 0) heartOpacities[lives] = 0; // Đảm bảo không nhỏ hơn 0

                // Cập nhật độ mờ của hình trái tim
                Color color = Color.FromArgb((int)(heartOpacities[lives] * 255), 255, 0, 0); // Đỏ
                hearts[lives].BackColor = color; // Thay đổi màu sắc với độ mờ
            }
        }


        private void UpdateLivesLabel()
        {
            lblLives.Text = $"Mạng: {lives}";

            // Đổi màu nếu còn ít mạng
            if (lives == 1)
            {
                lblLives.ForeColor = Color.Red; // Cảnh báo màu đỏ
            }
        }

        private void UpdateScoreLabel()
        {
            lblScore.Text = $"Điểm: {score}";

            // Đổi màu điểm ngẫu nhiên mỗi khi điểm là bội số của 5
            if (score % 5 == 0)
            {
                lblScore.ForeColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }

            // Kiểm tra nếu đạt đủ điểm để lên cấp
            if (score >= nextLevelScore)
            {
                currentLevel++;          // Tăng cấp độ.
                nextLevelScore += 15;     // Thiết lập mốc điểm cho level kế tiếp.
                UpdateLevel();            // Cập nhật thông số cho level mới.
            }

            // Cập nhật hiển thị level
            lblScore.Text += $" | Level: {currentLevel}";
        }



        private void UpdateLevel()
        {
            // Tăng tốc độ rơi và di chuyển cho độ khó mới.
            yDeltaEgg += 2;        // Tăng tốc độ rơi trứng.
            xDeltaBasket += 2;     // Tăng tốc độ di chuyển rổ.
            xDeltaChicken += 1;    // Tăng tốc độ gà.

            // Giảm thời gian của các timer để tăng độ khó.
            tmEgg.Interval = Math.Max(5, tmEgg.Interval - 1);
            tmChicken.Interval = Math.Max(15, tmChicken.Interval - 2);

     
        }






        private void ResetGame()
        {
            score = 0;
            elapsedTime = 0;
            lives = 3; // Khôi phục số mạng
            heartOpacities = new float[3] { 1.0f, 1.0f, 1.0f }; // Khôi phục độ mờ trái tim

            UpdateScoreLabel();
            lblTime.Text = "Thời gian: 0s";

            // Hiển thị lại tất cả các trái tim với độ mờ mặc định
            foreach (var heart in hearts)
            {
                heart.BackColor = Color.FromArgb(255, 255, 0, 0); // Đặt độ mờ ban đầu là 100%
            }

            ResetEgg();
            ResetGoldEgg();
            xBasket = (this.ClientSize.Width - pbBasket.Width) / 2;
            pbBasket.Location = new Point(xBasket, yBasket);

            isGameOver = false;

            tmEgg.Start();
            tmChicken.Start();
            tmBasketMovement.Start();
        }




        private void Form27wp_KeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver) return;

            if (e.KeyCode == Keys.Right) moveRight = true;
            if (e.KeyCode == Keys.Left) moveLeft = true;
        }

        private void Form27wp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right) moveRight = false;
            if (e.KeyCode == Keys.Left) moveLeft = false;
        }

        private void TmBasketMovement_Tick(object sender, EventArgs e)
        {
            if (moveRight && (xBasket < this.ClientSize.Width - pbBasket.Width))
            {
                xBasket += xDeltaBasket;
                pbBasket.Location = new Point(xBasket, yBasket);
            }
            if (moveLeft && xBasket > 0)
            {
                xBasket -= xDeltaBasket;
                pbBasket.Location = new Point(xBasket, yBasket);
            }
        }

        private void tmEgg_Tick(object sender, EventArgs e)
        {
            HandleGoldEgg();
            HandleNormalEgg();
            CreateEggs();
        }

        private void HandleGoldEgg()
        {
            if (isGoldEggFalling)
            {
                yGoldEgg += yDeltaEgg;
                pbGoldEgg.Location = new Point(xGoldEgg, yGoldEgg);
                pbGoldEgg.Visible = true;

                if (pbBasket.Bounds.IntersectsWith(pbGoldEgg.Bounds) && yGoldEgg + pbGoldEgg.Height >= yBasket)
                {
                    score += 2; // Cộng 2 điểm cho trứng vàng
                    UpdateScoreLabel(); // Cập nhật hiển thị điểm
                    ShowScoreEffect("+2");
                    PlayGoldEggSound();
                    ResetGoldEgg();
                }
                else if (yGoldEgg > this.ClientSize.Height)
                {
                    ResetGoldEgg();
                }
            }
        }

        //private void HandleNormalEgg()
        //{
        //    if (isNormalEggFalling)
        //    {
        //        yEgg += yDeltaEgg;
        //        pbEgg.Location = new Point(xEgg, yEgg);
        //        pbEgg.Visible = true;

        //        if (pbBasket.Bounds.IntersectsWith(pbEgg.Bounds) && yEgg + pbEgg.Height >= yBasket)
        //        {
        //            score += 1; // Cộng 1 điểm cho trứng thường
        //            UpdateScoreLabel(); // Cập nhật hiển thị điểm
        //            ShowScoreEffect("+1");
        //            ResetEgg();
        //        }

        //        if (yEgg > this.ClientSize.Height)
        //        {
        //            isGameOver = true;
        //            ShowGameOverMessage(); // Gọi phương thức hiển thị thông báo thua
        //        }
        //    }
        //}


        private void CreateEggs()
        {
            if (!isGoldEggFalling && !isNormalEggFalling)
            {
                if (random.Next(100) < 20)
                {
                    StartGoldEgg();
                }
                else if (random.Next(100) < 80)
                {
                    StartNormalEgg();
                }
            }
        }

        private void StartGoldEgg()
        {
            isGoldEggFalling = true;
            yGoldEgg = 10;
            xGoldEgg = xChicken + (pbChicken.Width - pbGoldEgg.Width) / 2;
            yGoldEgg = yChicken + pbChicken.Height + 5;
            pbGoldEgg.Location = new Point(xGoldEgg, yGoldEgg);
        }

        private void StartNormalEgg()
        {
            isNormalEggFalling = true;
            yEgg = 10;
            xEgg = xChicken + (pbChicken.Width - pbEgg.Width) / 2;
            yEgg = yChicken + pbChicken.Height + 5;
            pbEgg.Location = new Point(xEgg, yEgg);
        }

        private void tmChicken_Tick(object sender, EventArgs e)
        {
            // Tăng xác suất đổi hướng để gà không đứng yên quá lâu
            if (random.Next(100) < 15) // 15% xác suất đổi hướng
            {
                xDeltaChicken = random.Next(-10, 11); // Tốc độ ngẫu nhiên từ -10 đến 10
            }

            // Kiểm tra nếu gà chạm vào biên form và đảo chiều
            if (xChicken + xDeltaChicken >= this.ClientSize.Width - pbChicken.Width || xChicken + xDeltaChicken <= 0)
            {
                xDeltaChicken = -xDeltaChicken;
            }

            // Cập nhật vị trí gà
            xChicken += xDeltaChicken;

            // Đảm bảo gà không ra ngoài màn hình
            if (xChicken < 0) xChicken = 0;
            if (xChicken > this.ClientSize.Width - pbChicken.Width) xChicken = this.ClientSize.Width - pbChicken.Width;

            pbChicken.Location = new Point(xChicken, yChicken);
        }


        private void ShowScoreEffect(string scoreText)
        {
            lblScoreEffect.Text = scoreText;
            lblScoreEffect.Location = new Point(pbBasket.Left + 10, pbBasket.Top - 30);
            lblScoreEffect.Visible = true;
            tmScoreEffect.Start();
        }
        private void PlayGoldEggSound()
        {
            try
            {
                goldEggSound.Play(); // Phát âm thanh khi bắt trứng vàng
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể phát âm thanh nhặt trứng vàng: {ex.Message}");
            }
        }

        private void TmScoreEffect_Tick(object sender, EventArgs e)
        {
            lblScoreEffect.Visible = false;
            tmScoreEffect.Stop();
        }
        private void PlayCatchSound()
        {
            try
            {
                soundPlayer.Play();// Phát âm thanh khi bắt trứng
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể phát âm thanh bắt trứng: {ex.Message}");
            }
        }

        private void PlayGameOverSound()
        {
            try
            {
                gameOverSound.Play(); // Phát âm thanh khi trò chơi kết thúc
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể phát âm thanh trò chơi kết thúc: {ex.Message}");
            }
        }

        private void ResetEgg()
        {
            pbEgg.Location = new Point(-100, -100);
            pbEgg.Visible = false;
            isNormalEggFalling = false;
        }

        private void ResetGoldEgg()
        {
            pbGoldEgg.Location = new Point(-100, -100);
            pbGoldEgg.Visible = false;
            isGoldEggFalling = false;
        }
        private void ShowGameOverMessage()
        {
            // Dừng tất cả các timer
            tmEgg.Stop();
            tmChicken.Stop();
            tmBasketMovement.Stop();
            PlayGameOverSound();
            StopBackgroundMusic();
            // Hiện thông báo với lựa chọn thoát hoặc chơi lại
            DialogResult result = MessageBox.Show(
                $"Bạn đã thua! Tổng điểm: {score}\nBạn có muốn chơi lại không?",
                "Game Over",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Xử lý kết quả từ thông báo
            if (result == DialogResult.Yes)
            {
                ResetGame(); // Chơi lại
                PlayBackgroundMusic();
            }
            else if (result == DialogResult.No)
            {
                this.Close(); // Thoát game
            }
        }




    }
}