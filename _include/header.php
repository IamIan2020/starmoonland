<div class="container-fluid">
    <div class="row flex align-items-center">
        <!-- logo -->
            <div class="logo"> <a href="<?php echo $WEB_ROOT ?>/index.php">
                        <img id="logo" src="<?php echo $WEB_ROOT ?>/_images/all/logo.svg" alt="星月大地">
                </a> </div>
        
            <!-- main-navigation start -->
            <div class="col-md-offset-2 col-md-10">
                <div class="main-navigation">
                <div class="nav-menu">
                    <ul class="nav navbar-nav">
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/about/index.php">星月故事<p>About</p></a>
                            <ul class="dropdown-menu">
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/about/index.php">星月緣起</a></li>
                                <li><a href="#">星月願景</a></li>
                                <li><a href="#">吉祥物介紹</a></li>
                            </ul>
                        </li>
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/news/index.php">訊息公告<p>News</p></a>
                            <ul class="dropdown-menu">
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/news/index.php">活動情報</a></li>
                                <li><a href="#">媒體報導</a></li>
                                <li><a href="#">其他公告</a></li>
                            </ul>
                        </li>
                        <li class="menu active"><a href="<?php echo $WEB_ROOT ?>/_pages/service/index.php">星月服務<p>Service</p></a>
                            <ul class="dropdown-menu">
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/service/index.php">大地營區</a></li>
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/service/02.php">大地烤肉</a></li>
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/service/03.php">大地風呂</a></li>
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/service/04.php">星月文旅</a></li>
                            </ul>
                        </li>
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/dining/index.php">餐飲宴會<p>Dining</p></a>
                            <ul class="dropdown-menu">
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/dining/index.php">景觀宴會館</a></li>
                                <li><a href="<?php echo $WEB_ROOT ?>/_pages/dining/02.php">景觀咖啡廳</a></li>
                                
                            </ul>
                        </li>
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/wedding/index.php">幸福婚宴<p>Wedding</p></a>
                        </li>
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/album/index.php">大地景觀<p>Album</p></a></li>
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/traffic/index.php">交通資訊<p>Traffic</p></a></li>
                        <li class="menu"><a href="<?php echo $WEB_ROOT ?>/_pages/contact/index.php">聯絡我們<p>Contact</p></a></li>
                    </ul>
                </div>
            </div>
            </div>
            
            <!-- /.main-navigation -->
        
        
    </div>
    <div class="mobile-menu">
        手機選單
    </div>
    <!-- /.mobile-menu -->
    <!-- Site Overlay -->
    <div class="site-overlay"></div>
    <?php include('pd-search.php')?>
</div>