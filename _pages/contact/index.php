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
    <link type="text/css" href="../../_css/contact.css" rel="stylesheet">
    <!-- InstanceEndEditable -->

    <!-- InstanceParam name="body_id" type="text" value="" -->
    <!-- InstanceParam name="body_class" type="text" value="about" -->
</head>

<body id="" class="son contact">
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
                <p class="en">Contact</p>
                <p>聯絡我們</p>
            </div>
            <img src="<?php echo $WEB_ROOT ?>/_images/contact/banner.png" alt="聯絡我們">
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
                    <div class="contact-box">
                       <p>親愛的客戶您好，承蒙您對星月大地休閒度假村的支持，歡迎您來信詢問任何問題或提供建議，以方便我們儘快針對您的問題做回覆，謝謝。</p>
                        <form action="" method="post" id="contact-form" class="form-horizontal contact-form" novalidate>
                            <fieldset>
                                    <section class="form-group">
                                        <label class="label col-sm-1">姓名<span class="red">*</span></label>
                                        <label class="input col-sm-11">
                                                <input type="text" name="name" id="name" class="form-control required">
                                            </label>
                                    </section>
                                    <section class="form-group">
                                        <label class="label col-sm-1">電話<span class="red">*</span></label>
                                        <label class="input col-sm-11">
                                                <input type="email" name="email" id="email" class="form-control required">
                                            </label>
                                    </section>
                                <section class="form-group">
                                    <label class="label col-sm-1">電子信箱<span class="red">*</span></label>
                                    <label class="input col-sm-11">
                                                <input type="email" name="email" id="email" class="form-control required">
                                            </label>
                                </section>
                                <section class="form-group">
                                    <label class="label col-sm-1">詢問事項<span class="red">*</span></label>
                                    <label class="textarea col-sm-11">
                                            <textarea rows="4" name="message" id="message" class="form-control required"></textarea>
                                        </label>
                                </section>

                                <section class="form-group">
                                    <label class="label col-sm-1">驗證碼<span class="red">*</span></label>
                                    <label class="input input-captcha col-sm-11">
                                          		 <img class="chkImg img" src="http://fakeimg.pl/97x30/ccc/" title="change" width="97" height="30" />
                                            	<input type="text" maxlength="4" name="chknum" id="chknum" class="form-control required">
                                        </label>
                                </section>
                                
                                <div class="btn-box">
                                    <button type="submit" class="btn">確認送出</button>
                                    <button type="reset" class="btn">清除重填</button>
                                </div>

                            </fieldset>

                        </form>

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