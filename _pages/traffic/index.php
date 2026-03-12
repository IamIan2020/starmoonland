<!DOCTYPE html>
<!--[if IE 9]> <html lang="en" class="ie9"> <![endif]-->
<!--[if !IE]><!-->
<html lang="zh-Hant-tw">
<!-- InstanceBegin template="/Templates/layout_php.dwt" codeOutsideHTMLIsLocked="false" -->
<!--<![endif]-->

<head>
    <meta charset="utf-8">
    <!-- InstanceBeginEditable name="head" -->
    <?php include('../../_include/head.php'); ?>
    <link type="text/css" href="../../_css/traffic.css" rel="stylesheet">
    <!-- InstanceEndEditable -->

    <!-- InstanceParam name="body_id" type="text" value="" -->
    <!-- InstanceParam name="body_class" type="text" value="about" -->
</head>

<body id="" class="son traffic">
    <!-- page wrapper start -->
    <div class="page-wrapper">
        <!-- header start -->
        <header class="header">
            <!-- InstanceBeginEditable name="header-menu" -->
            <?php include('../../_include/header.php'); ?>

            <!-- InstanceEndEditable -->
        </header>
        <!-- header end -->
        <!-- banner start -->
        <div class="banner">
            <!-- InstanceBeginEditable name="banner" -->
            <div class="title-son title-son-style style-brown style-w">
                <p class="en">Traffic</p>
                <p>交通資訊</p>
            </div>
            <img src="<?php echo $WEB_ROOT ?>/_images/traffic/banner.png" alt="交通資訊">
            <!-- InstanceEndEditable -->
        </div>
        <!-- banner end -->
        <!-- breadcramb start-->
        <div class="breadcramb">
            <!-- InstanceBeginEditable name="breadcramb" -->
            <?php include('../../_include/breadcrumb.php'); ?>
            <!-- InstanceEndEditable -->
        </div>
        <!-- breadcramb end -->
        <!-- main-container start -->
        <div class="main">
            <!-- InstanceBeginEditable name="main" -->
            <section class="content">
                <div class="container">
                    <div class="classbox">
                        <ul class="nav class-link">
                            <li role="presentation" class="active"><a href="#bus" aria-controls="bus" role="tab" data-toggle="tab">市區公車</a></li>
                            <li role="presentation"><a href="#driving" aria-controls="driving" role="tab" data-toggle="tab">自行開車</a></li>
                        </ul>
                    </div>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="bus">
                            <div class="row">
                                <div class="col-md-6">
                                    <ul class="star">
                                        <li><h4>豐原客運214號：</h4>后里馬場或大甲火車站發車→六分月眉北路口站牌下車 →步行約5分鐘抵達</li>
                                        <li><h4>豐原客運215號：</h4>豐原或大甲體育場發車→麗寶樂園站下車→步行約20分鐘抵達</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <ul class="star">
                                        <li><h4>統聯客運155號：</h4>高鐵台中站或麗寶樂園發車→麗寶樂園站下車→步行約20分鐘抵達</li>
                                        <li><h4>統聯客運155副線：</h4>高鐵台中站或麗寶樂園發車→月眉北路口站牌下車→步行約15分鐘抵達</li>
                                    </ul>
                                </div>
                            </div>
                            
                        </div>
                        <div role="tabpanel" class="tab-pane" id="driving">
                           <div class="row">
                                <div class="col-md-6">
                                    <ul class="star">
                                        <li><h4>走國道1號：</h4>國1下→往外埔（大摩天輪）方向→約3分鐘車程即可抵達</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <ul class="star">
                                        <li><h4>走國道3號：</h4>國3下→往后里（大摩天輪）方向→約5至10分鐘車程可抵達</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="map iframe-rwd">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d14541.597505171469!2d120.701191!3d24.332577!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0xf22f690b198e3e44!2z5pif5pyI5aSn5Zyw5LyR6ZaS5LqL5qWt5pyJ6ZmQ5YWs5Y-4!5e0!3m2!1sen!2shk!4v1465960095168" frameborder="0" style="border:0"></iframe>
                    </div>
                </div>
            </section>
            <!-- InstanceEndEditable -->
        </div>
        <!-- main-container end -->
        <!-- footer start -->
        <footer id="footer clearfix">
            <!-- InstanceBeginEditable name="footer" -->
            <?php include('../../_include/footer.php'); ?>
            <!-- InstanceEndEditable -->
        </footer>
        <!-- footer end -->
    </div>
    <!-- page-wrapper end -->
    <!-- JavaScript files -->
    <!-- InstanceBeginEditable name="js" -->
    <?php include('../../_include/js.php'); ?>
    <!-- InstanceEndEditable -->
</body>
<!-- InstanceEnd -->

</html>