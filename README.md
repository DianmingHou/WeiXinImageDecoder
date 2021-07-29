# WeiXinImageDecoder
解码微信电脑版(微信PC版)图片缓存
参考文章[解密微信电脑版image文件夹下缓存的用户图片 dat文件解码解密查看方法](https://blog.csdn.net/a386115360/article/details/103215560)

# 程序参数说明  
三个参数，
 - 第一个为该用户的微信文件夹，默认C:\Users\[系统用户名]\Documents\WeChat Files\wxid_[编码]\FileStorage\Image
 - 第二个参数为目标文件夹
 - 第三个参数为dat文件16进制首字节，并不是所有文件都满足，多查看几个找一个通用的。
