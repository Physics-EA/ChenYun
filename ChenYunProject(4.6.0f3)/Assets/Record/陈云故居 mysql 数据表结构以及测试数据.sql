/*
Navicat MySQL Data Transfer

Source Server         : 123
Source Server Version : 50714
Source Host           : localhost:3306
Source Database       : mysql

Target Server Type    : MYSQL
Target Server Version : 50714
File Encoding         : 65001

Date: 2016-11-22 11:05:25
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `cygj_alert_process_log`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_alert_process_log`;
CREATE TABLE `cygj_alert_process_log` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `ALERTTIME` varchar(20) DEFAULT NULL,
  `ALERTTYPE` varchar(20) DEFAULT NULL,
  `PERSON` varchar(20) DEFAULT NULL,
  `STARTTIME` varchar(20) DEFAULT NULL,
  `ENDTIME` varchar(20) DEFAULT NULL,
  `MEMO` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=238 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_alert_process_log
-- ----------------------------
INSERT INTO `cygj_alert_process_log` VALUES ('217', '2016/01/22 11:34:00', '局部人员拥堵', '管理员', '2016/01/22 11:34:13', '2016/01/22 11:35:37', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('219', '2016/01/22 13:45:25', '局部人员拥堵', '管理员', '2016/01/22 13:45:27', '2016/01/22 13:45:45', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('221', '2016/01/26 10:52:01', '局部人员拥堵', '管理员', '2016/01/26 10:52:03', '2016/01/26 10:53:22', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('223', '2016/01/26 10:53:59', '局部人员拥堵', '管理员', '2016/01/26 10:54:02', null, null);
INSERT INTO `cygj_alert_process_log` VALUES ('225', '2016/01/26 11:35:04', '局部人员拥堵', null, '2016/01/26 11:35:16', '2016/01/26 11:35:17', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('227', '2016/01/26 12:08:56', '局部人员拥堵', null, '2016/01/26 12:08:58', '2016/01/26 12:09:20', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('229', '2016/01/26 13:31:07', '局部人员拥堵', null, '2016/01/26 13:31:08', '2016/01/26 13:32:03', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('231', '2016/01/26 13:58:37', '局部人员拥堵', null, '2016/01/26 13:58:39', '2016/01/26 13:58:48', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('233', '2016/01/26 14:05:05', '局部人员拥堵', null, '2016/01/26 14:05:08', '2016/01/26 14:05:53', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('234', '2016/08/29 10:56:45', '局部人员拥堵', '管理员', '2016/08/29 10:57:12', '2016/08/29 10:57:16', '处理完成');
INSERT INTO `cygj_alert_process_log` VALUES ('235', '2016/09/18 11:40:48', '局部人员拥堵', '', '2016/09/18 11:40:58', '2016/09/18 11:40:58', '误报警');
INSERT INTO `cygj_alert_process_log` VALUES ('236', '2016/09/18 11:40:56', '局部人员拥堵', '', '2016/09/18 11:40:58', '2016/09/18 11:40:58', '误报警');
INSERT INTO `cygj_alert_process_log` VALUES ('237', '2016/11/16 17:02:25', '新建疏散预案1', '', '', '', '');

-- ----------------------------
-- Table structure for `cygj_authority`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_authority`;
CREATE TABLE `cygj_authority` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `AUTHORITY` varchar(100) DEFAULT NULL,
  `DESCRIPTION` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_authority
-- ----------------------------
INSERT INTO `cygj_authority` VALUES ('1', 'UserInfoBrowse', '用户信息查看');
INSERT INTO `cygj_authority` VALUES ('2', 'UserInfoManagement', '用户修改');
INSERT INTO `cygj_authority` VALUES ('3', 'UserGroupInfoBrowse', '用户组信息查看');
INSERT INTO `cygj_authority` VALUES ('4', 'UserGroupManagement', '用户组修改');
INSERT INTO `cygj_authority` VALUES ('5', 'EvacuatePlanManagement', '巡航预案管理');
INSERT INTO `cygj_authority` VALUES ('6', 'DeviceManagement', '设备管理');
INSERT INTO `cygj_authority` VALUES ('7', 'HistoryManagement', '日志管理');

-- ----------------------------
-- Table structure for `cygj_device`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_device`;
CREATE TABLE `cygj_device` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `GUID` varchar(100) DEFAULT NULL COMMENT '真实摄像头GUID',
  `NAME` varchar(200) DEFAULT NULL COMMENT '名称',
  `DESCRIPTION` varchar(200) DEFAULT NULL COMMENT '显示用描述',
  `STATUS` varchar(4) DEFAULT NULL,
  `POSX` double(15,5) DEFAULT NULL,
  `POSY` double(15,5) DEFAULT NULL,
  `POSZ` double(15,5) DEFAULT NULL,
  `CAMERATAG` varchar(100) DEFAULT NULL,
  `ROTATEPOINTPOSX` varchar(20) DEFAULT NULL,
  `ROTATEPOINTPOSY` varchar(20) DEFAULT NULL,
  `ROTATEPOINTPOSZ` varchar(20) DEFAULT NULL,
  `CAMERAPOSX` varchar(20) DEFAULT NULL,
  `CAMERAPOSY` varchar(20) DEFAULT NULL,
  `CAMERAPOSZ` varchar(20) DEFAULT NULL,
  `CAMERAROTATEX` varchar(20) DEFAULT NULL,
  `CAMERAROTATEY` varchar(20) DEFAULT NULL,
  `CAMERAROTATEZ` varchar(20) DEFAULT NULL,
  `MONITORRADIO` double(15,5) DEFAULT NULL,
  `MONITORSCOPE` double(15,5) DEFAULT NULL,
  `OFFSETANGLE` double(15,5) DEFAULT NULL,
  `RTSPURL` varchar(255) DEFAULT NULL,
  `USERTSP` char(1) DEFAULT NULL,
  `PASSENGERFLOWURL` varchar(255) DEFAULT '0:127.0.0.1' COMMENT '客流统计服务器对应的地址',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_device
-- ----------------------------
INSERT INTO `cygj_device` VALUES ('1', '5-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ04', '门口', '正常', '-13.05300', '2.22400', '65.74300', 'Camera1', '-3.47000', '0.00000', '-2.70000', '-3.47000', '60.00000', '-87.95000', '35.13836', '0.00000', '0.00000', '22.00000', '106.00000', '45.00000', 'rtsp://admin:12345-12345@192.168.0.187:554/ch1', '0', '0:172.22.20.233');
INSERT INTO `cygj_device` VALUES ('2', '21-0-0-0-0-101-0-168-192-135-21', '192.168.0.101-��ͷ08', '后门', '正常', '-21.24812', '13.59971', '-21.02454', 'Camera2', '-19.30619', '0.00000', '3.49948', '-104.42320', '60.00000', '-1.25932', '35.13837', '86.80000', '0.00000', '28.00000', '36.00000', '53.00000', 'rtsp://admin:admin@192.168.0.186:554', '0', '1:172.22.20.233');
INSERT INTO `cygj_device` VALUES ('3', '1-0-0-0-0-188-173-40-196-16-32', '192.168.0.187-��ͷ01', '屋檐', '正常', '-20.89694', '13.84337', '21.06748', 'Camera3', '-8.96168', '0.00000', '8.91641', '-5.83783', '60.00000', '94.10909', '35.13838', '182.10000', '0.00000', '26.00000', '58.00000', '0.00000', 'rtsp://admin:admin@192.168.0.151:37777', '0', '2:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('4', '1-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ01', 'Camera6', '正常', '78.89600', '2.04900', '-118.09400', 'Camera6', '67.64540', '0.00000', '-100.37630', '97.95045', '38.84965', '-146.51170', '35.13852', '326.70030', '0.00000', '18.00000', '61.00000', '79.00000', '', '0', '0:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('5', '1-0-0-0-0-60-239-140-200-115-105', '192.168.0.186-��ͷ01', 'Camera7', '正常', '93.18300', '3.35300', '-110.92200', 'Camera7', '99.12003', '0.00000', '-88.25509', '62.90634', '38.84965', '-129.91380', '35.13852', '41.00027', '0.00000', '14.00000', '76.00000', '216.00000', '', '0', '3:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('6', '1-0-0-0-0-188-173-40-196-16-32', '192.168.0.187-��ͷ01', 'Camera9', '正常', '41.60800', '3.43000', '-83.25000', 'Camera9', '29.71544', '0.00000', '-71.68672', '65.63699', '38.84965', '-113.59750', '35.13854', '319.40030', '0.00000', '12.00000', '98.00000', '87.00000', '', '0', '0:127.0.0.1');
INSERT INTO `cygj_device` VALUES ('7', '3-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ02', 'Camera13', '正常', '-15.08700', '2.28100', '-127.93400', 'Camera13', '-13.77215', '0.00000', '-137.29670', '1.03028', '27.33855', '-101.38430', '35.13845', '202.40060', '0.00000', '14.00000', '75.00000', '301.00000', '', '0', '0:127.0.0.1');
INSERT INTO `cygj_device` VALUES ('8', '20-0-0-0-0-101-0-168-192-135-21', '192.168.0.101-��ͷ07', 'Camera15', '正常', '258.78800', '3.31500', '-20.14500', 'Camera15', '252.82120', '0.00000', '-26.77591', '281.60470', '30.21640', '5.07847', '35.13842', '222.10080', '0.00000', '13.00000', '85.00000', '289.00000', '', '0', '2:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('9', '4-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ03', 'Camera10', '正常', '-215.40200', '5.77000', '-111.29600', 'Camera10', '-204.22980', '0.00000', '-110.74390', '-237.17680', '31.65520', '-80.12723', '35.13852', '132.90040', '0.00000', '11.00000', '60.00000', '151.00000', '', '0', '2:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('10', '17-0-0-0-0-101-0-168-192-135-21', '192.168.0.101-��ͷ05', 'Camera17', '正常', '315.57900', '3.43500', '7.46700', 'Camera17', '305.00620', '0.00000', '9.66866', '343.82040', '30.21640', '-8.67943', '35.13841', '295.30090', '0.00000', '10.00000', '123.00000', '179.00000', '', '0', '1:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('11', '19-0-0-0-0-101-0-168-192-135-21', '192.168.0.101-��ͷ06', 'Camera8', '正常', '55.34200', '2.11300', '-105.29300', 'Camera8', '57.62331', '0.00000', '-88.43471', '21.40963', '38.84965', '-130.09340', '35.13853', '41.00027', '0.00000', '12.00000', '72.00000', '258.00000', '', '0', '3:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('12', '5-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ04', 'Camera12', '正常', '-283.04400', '5.59700', '-143.36000', 'Camera12', '-269.03570', '0.00000', '-136.25800', '-309.52910', '31.65520', '-155.83300', '35.13848', '64.20039', '0.00000', '12.00000', '49.00000', '186.00000', '', '0', '0:127.0.0.1');
INSERT INTO `cygj_device` VALUES ('13', '1-0-0-0-0-60-239-140-200-115-105', '192.168.0.186-��ͷ01', 'Camera5', '正常', '165.42500', '2.10400', '-99.76100', 'Camera5', '171.11180', '0.00000', '-88.48662', '133.32560', '38.84965', '-128.72440', '35.13855', '43.20031', '0.00000', '9.00000', '63.00000', '237.00000', '', '0', '0:127.0.0.1');
INSERT INTO `cygj_device` VALUES ('14', '1-0-0-0-0-188-173-40-196-16-32', '192.168.0.187-��ͷ01', 'Camera14', '正常', '16.88800', '3.38500', '-116.15000', 'Camera14', '17.09595', '0.00000', '-127.69530', '31.89838', '27.33855', '-91.78304', '35.13848', '202.40060', '0.00000', '10.00000', '64.00000', '324.00000', '', '0', '0:127.0.0.1');
INSERT INTO `cygj_device` VALUES ('15', '3-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ02', 'Camera11', '正常', '-243.36400', '5.66500', '-116.43300', 'Camera11', '-236.47060', '0.00000', '-105.24630', '-258.20720', '31.65520', '-144.62150', '35.13851', '28.90036', '0.00000', '17.00000', '67.00000', '56.00000', '', '0', '1:192.168.0.201');
INSERT INTO `cygj_device` VALUES ('16', '3-0-0-0-0-108-1-168-192-21-136', 'Camerac16', 'Camerac16', '正常', '312.77500', '2.23800', '-17.09500', 'Camera16', '300.22130', '0.00000', '-20.23492', '334.82210', '30.21640', '5.18111', '35.13837', '233.70090', '0.00000', '15.00000', '82.00000', '252.00000', '', '0', '0:127.0.0.1');
INSERT INTO `cygj_device` VALUES ('17', '4-0-0-0-0-108-1-168-192-21-136', '192.168.0.101-��ͷ03', 'Camera4', '正常', '255.89000', '3.41700', '7.14300', 'Camera4', '263.85790', '0.00000', '16.47098', '226.07180', '38.84965', '-23.76680', '35.13858', '43.20021', '0.00000', '14.00000', '64.00000', '65.00000', '', '0', '0:127.0.0.1');

-- ----------------------------
-- Table structure for `cygj_evacuate_area`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_evacuate_area`;
CREATE TABLE `cygj_evacuate_area` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `fontsize` int(11) DEFAULT NULL,
  `points` varchar(1000) DEFAULT NULL,
  `cameralist` varchar(200) DEFAULT NULL COMMENT '绑定摄像机列表',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 COMMENT='疏散区域表';

-- ----------------------------
-- Records of cygj_evacuate_area
-- ----------------------------
INSERT INTO `cygj_evacuate_area` VALUES ('10', '新建疏散区域1', '20', '-15.69682|2.333405E-07|-31.10775|13.65387|4.105938E-07|-32.3712|7.995404|1.634432E-06|-46.3727|7.995404|1.634432E-06|-46.3727|-7.454539|1.814909E-06|-48.51446|-15.69682|2.333405E-07|-31.10775', '1|5');

-- ----------------------------
-- Table structure for `cygj_evacuate_areaofplan`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_evacuate_areaofplan`;
CREATE TABLE `cygj_evacuate_areaofplan` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `evacuateplanid` int(11) DEFAULT NULL COMMENT '疏散预案id',
  `evacuateareaid` int(11) DEFAULT NULL COMMENT '疏散区域id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='疏散预案疏散区域关联表';

-- ----------------------------
-- Records of cygj_evacuate_areaofplan
-- ----------------------------
INSERT INTO `cygj_evacuate_areaofplan` VALUES ('6', '28', '10');

-- ----------------------------
-- Table structure for `cygj_evacuate_plan`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_evacuate_plan`;
CREATE TABLE `cygj_evacuate_plan` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL COMMENT '疏散预案名称',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8 COMMENT='疏散预案';

-- ----------------------------
-- Records of cygj_evacuate_plan
-- ----------------------------
INSERT INTO `cygj_evacuate_plan` VALUES ('28', '新建疏散预案1');

-- ----------------------------
-- Table structure for `cygj_group`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_group`;
CREATE TABLE `cygj_group` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `NAME` varchar(100) DEFAULT NULL,
  `STATUS` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_group
-- ----------------------------
INSERT INTO `cygj_group` VALUES ('1', '普通管理员', '正常');
INSERT INTO `cygj_group` VALUES ('3', '普通员工', '正常');
INSERT INTO `cygj_group` VALUES ('10', '超级管理员', '正常');
INSERT INTO `cygj_group` VALUES ('11', '临时管理员', '正常');

-- ----------------------------
-- Table structure for `cygj_group_authority`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_group_authority`;
CREATE TABLE `cygj_group_authority` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `GROUPID` int(10) DEFAULT NULL,
  `AUTHORITYID` int(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_group_authority
-- ----------------------------
INSERT INTO `cygj_group_authority` VALUES ('18', '10', '1');
INSERT INTO `cygj_group_authority` VALUES ('19', '10', '2');
INSERT INTO `cygj_group_authority` VALUES ('20', '10', '3');
INSERT INTO `cygj_group_authority` VALUES ('21', '10', '4');
INSERT INTO `cygj_group_authority` VALUES ('22', '10', '5');
INSERT INTO `cygj_group_authority` VALUES ('23', '10', '6');
INSERT INTO `cygj_group_authority` VALUES ('24', '10', '7');
INSERT INTO `cygj_group_authority` VALUES ('57', '1', '1');
INSERT INTO `cygj_group_authority` VALUES ('58', '1', '2');
INSERT INTO `cygj_group_authority` VALUES ('59', '1', '3');
INSERT INTO `cygj_group_authority` VALUES ('61', '11', '1');

-- ----------------------------
-- Table structure for `cygj_passengerflow_area`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_passengerflow_area`;
CREATE TABLE `cygj_passengerflow_area` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'id',
  `NAME` varchar(255) CHARACTER SET utf8 DEFAULT '' COMMENT '客流统计区域名称',
  `CAMERAIDLST` varchar(255) DEFAULT NULL,
  `POINTS` varchar(3600) DEFAULT NULL,
  `WARNLEVEL1` int(11) DEFAULT '0' COMMENT '一级警戒人数',
  `WARNLEVEL2` int(11) DEFAULT '0' COMMENT '二级警戒人数',
  `WARNLEVEL3` int(11) DEFAULT '0' COMMENT '三级警戒人数',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of cygj_passengerflow_area
-- ----------------------------
INSERT INTO `cygj_passengerflow_area` VALUES ('15', '主客流', '', '', '0', '0', '0');
INSERT INTO `cygj_passengerflow_area` VALUES ('17', '新建客流统计区域2', '1,1,0,0,0', '200.2494|-4.20274E-06|44.79679|188.4952|-1.805794E-06|-17.64137|272.0578|-4.646917E-06|51.28041|272.0578|-4.646917E-06|51.28041|222.7339|-1.76911E-05|85.51484|200.2494|-4.20274E-06|44.79679', '0', '0', '0');
INSERT INTO `cygj_passengerflow_area` VALUES ('18', '新建客流统计区域3', '1,0,0,1,0|2,0,1,0,0', '-14.03336|-1.50784E-08|-27.46695|18.9824|6.248354E-08|-28.36785|-3.747139|1.769318E-06|-48.09997', '0', '0', '0');

-- ----------------------------
-- Table structure for `cygj_passengerflow_log`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_passengerflow_log`;
CREATE TABLE `cygj_passengerflow_log` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `DATETIME` varchar(16) DEFAULT NULL COMMENT '数据存入日期',
  `PASSENGERFLOWURL` varchar(20) DEFAULT NULL COMMENT '客流信息来源',
  `SUMCOUNT` varchar(45) DEFAULT NULL COMMENT '当前人数',
  `ENTERCOUNT` varchar(45) DEFAULT NULL COMMENT '进入人数',
  `EXITCOUNT` varchar(45) DEFAULT NULL COMMENT '离开人数',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8 COMMENT='客流统计信息日志';

-- ----------------------------
-- Records of cygj_passengerflow_log
-- ----------------------------
INSERT INTO `cygj_passengerflow_log` VALUES ('1', '20160914151949', '0:172.22.20.233', '18', '18', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('2', '20160914151949', '1:172.22.20.233', '0', '0', '18');
INSERT INTO `cygj_passengerflow_log` VALUES ('3', '20160914152051', '0:172.22.20.233', '5', '5', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('4', '20160914152051', '1:172.22.20.233', '0', '0', '5');
INSERT INTO `cygj_passengerflow_log` VALUES ('5', '20160914155115', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('6', '20160914155115', '1:172.22.20.233', '0', '0', '12');
INSERT INTO `cygj_passengerflow_log` VALUES ('7', '20160914155138', '1:172.22.20.233', '0', '0', '7');
INSERT INTO `cygj_passengerflow_log` VALUES ('8', '20160914155138', '0:172.22.20.233', '6', '6', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('9', '20160914155510', '1:172.22.20.233', '0', '0', '8');
INSERT INTO `cygj_passengerflow_log` VALUES ('10', '20160914155510', '0:172.22.20.233', '7', '7', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('11', '20160914155617', '1:172.22.20.233', '0', '0', '34');
INSERT INTO `cygj_passengerflow_log` VALUES ('12', '20160914155617', '0:172.22.20.233', '31', '31', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('13', '20160914155716', '1:172.22.20.233', '0', '0', '42');
INSERT INTO `cygj_passengerflow_log` VALUES ('14', '20160914155716', '0:172.22.20.233', '37', '37', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('25', '20160920144521', '1:172.22.20.233', '0', '0', '50');
INSERT INTO `cygj_passengerflow_log` VALUES ('26', '20160920144521', '0:172.22.20.233', '46', '46', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('27', '20160920172405', '1:172.22.20.233', '0', '0', '50');
INSERT INTO `cygj_passengerflow_log` VALUES ('28', '20160920172405', '0:172.22.20.233', '46', '46', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('29', '20160920172545', '1:172.22.20.233', '0', '0', '50');
INSERT INTO `cygj_passengerflow_log` VALUES ('30', '20160920172545', '0:172.22.20.233', '46', '46', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('31', '20160929134852', '1:172.22.20.233', '0', '0', '5');
INSERT INTO `cygj_passengerflow_log` VALUES ('32', '20160929134853', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('33', '20160929134950', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('34', '20160929135048', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('35', '20160929135158', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('36', '20160929135633', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('37', '20160929143715', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('38', '20160929162052', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('39', '20160929162418', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('40', '20160929163250', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('41', '20160929163336', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('42', '20160929163611', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('43', '20160929170022', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('44', '20160929170234', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('45', '20160929170351', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('46', '20160929170511', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('47', '20160929170627', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('48', '20160929170859', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('49', '20160929172646', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('50', '20160929173024', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('51', '20160929173626', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('52', '20160929174122', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('53', '20160929174710', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('54', '20160929180432', '0:172.22.20.233', '13', '13', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('55', '20160930104919', '0:172.22.20.233', '30', '30', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('56', '20160930105029', '0:172.22.20.233', '51', '51', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('57', '20160930112008', '0:172.22.20.233', '453', '453', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('58', '20160930112008', '1:172.22.20.233', '217', '217', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('59', '20160930112008', '2:172.22.20.233', '193', '193', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('60', '20160930112008', '3:172.22.20.233', '153', '153', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('61', '20160930124158', '0:172.22.20.233', '1415', '1415', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('62', '20160930124158', '1:172.22.20.233', '217', '217', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('63', '20160930124158', '2:172.22.20.233', '193', '193', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('64', '20160930124158', '3:172.22.20.233', '153', '153', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('65', '20160930124508', '0:172.22.20.233', '1479', '1479', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('66', '20160930124508', '1:172.22.20.233', '217', '217', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('67', '20160930124508', '2:172.22.20.233', '193', '193', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('68', '20160930124508', '3:172.22.20.233', '153', '153', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('69', '20160930130358', '0:172.22.20.233', '1508', '1508', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('70', '20160930130358', '1:172.22.20.233', '217', '217', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('71', '20160930130358', '2:172.22.20.233', '193', '193', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('72', '20160930130358', '3:172.22.20.233', '153', '153', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('73', '20160930135555', '0:172.22.20.233', '1508', '1508', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('74', '20160930135555', '1:172.22.20.233', '217', '217', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('75', '20160930135555', '2:172.22.20.233', '193', '193', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('76', '20160930135555', '3:172.22.20.233', '153', '153', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('77', '20161024162417', '1:172.22.20.233', '22', '22', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('78', '20161024162417', '0:172.22.20.233', '19', '19', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('79', '20161024162857', '1:172.22.20.233', '102', '102', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('80', '20161024162857', '0:172.22.20.233', '86', '86', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('81', '20161024163026', '1:172.22.20.233', '129', '129', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('82', '20161024163026', '0:172.22.20.233', '110', '110', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('83', '20161104141248', '0:172.22.20.233', '24', '24', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('84', '20161104141248', '1:172.22.20.233', '29', '29', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('85', '20161104143607', '0:172.22.20.233', '34', '34', '0');
INSERT INTO `cygj_passengerflow_log` VALUES ('86', '20161104143607', '1:172.22.20.233', '40', '40', '0');

-- ----------------------------
-- Table structure for `cygj_preset_position`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_preset_position`;
CREATE TABLE `cygj_preset_position` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `DEVICEID` int(10) DEFAULT NULL,
  `NAME` varchar(100) DEFAULT NULL,
  `KEEPWATCH` char(2) DEFAULT NULL,
  `DESCRIPTION` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_preset_position
-- ----------------------------
INSERT INTO `cygj_preset_position` VALUES ('29', '1', '1', '是', '请重新设定守望位的信息');
INSERT INTO `cygj_preset_position` VALUES ('30', '1', '2', '否', '请重新设定预设位1的信息');
INSERT INTO `cygj_preset_position` VALUES ('31', '1', '3', '否', '请重新设定预设位2的信息');
INSERT INTO `cygj_preset_position` VALUES ('32', '1', '4', '否', '请重新设定预设位3的信息');
INSERT INTO `cygj_preset_position` VALUES ('33', '1', '5', '否', '请重新设定预设位4的信息');
INSERT INTO `cygj_preset_position` VALUES ('34', '1', '6', '否', '请重新设定预设位5的信息');
INSERT INTO `cygj_preset_position` VALUES ('35', '3', '1', '是', '请重新设定守望位的信息');
INSERT INTO `cygj_preset_position` VALUES ('36', '3', '2', '否', '请重新设定预设位1的信息');
INSERT INTO `cygj_preset_position` VALUES ('37', '3', '3', '否', '请重新设定预设位2的信息');
INSERT INTO `cygj_preset_position` VALUES ('38', '3', '4', '否', '请重新设定预设位3的信息');
INSERT INTO `cygj_preset_position` VALUES ('39', '3', '5', '否', '请重新设定预设位4的信息');
INSERT INTO `cygj_preset_position` VALUES ('40', '3', '6', '否', '请重新设定预设位5的信息');

-- ----------------------------
-- Table structure for `cygj_user_basic`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_user_basic`;
CREATE TABLE `cygj_user_basic` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `USERNAME` varchar(100) DEFAULT NULL,
  `PASSWORD` varchar(100) DEFAULT NULL,
  `REALNAME` varchar(100) DEFAULT NULL,
  `TELPHONE` varchar(100) DEFAULT NULL,
  `CREATETIME` varchar(100) DEFAULT NULL,
  `ADDRESS` varchar(100) DEFAULT NULL,
  `STATUS` varchar(100) DEFAULT NULL,
  `MEMO` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=73 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_user_basic
-- ----------------------------
INSERT INTO `cygj_user_basic` VALUES ('1', 'admin', 'admin1234', '管理员', '15816526085', '2015年11月14日09:36:26', '上海市徐汇区钦州北路', '正常', '超级用户');
INSERT INTO `cygj_user_basic` VALUES ('69', 'test', '1234', '测试而已', '1234567890', '2016/07/26 14:00', '地址', '0', '测试数据而已');
INSERT INTO `cygj_user_basic` VALUES ('71', 'test', '1234', '测试而已', '1234567890', '2016/07/26 14:00', '地址', '0', '测试数据而已');
INSERT INTO `cygj_user_basic` VALUES ('72', '11', '1234567', 'test', '13917231812', '2016年08月26日 17:08:45', 'test', '正常', 'test');

-- ----------------------------
-- Table structure for `cygj_user_group`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_user_group`;
CREATE TABLE `cygj_user_group` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `USERID` int(10) DEFAULT NULL,
  `GROUPID` int(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_user_group
-- ----------------------------
INSERT INTO `cygj_user_group` VALUES ('1', '1', '10');
INSERT INTO `cygj_user_group` VALUES ('16', '72', '3');

-- ----------------------------
-- Table structure for `cygj_video_patrol_detail_log`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_video_patrol_detail_log`;
CREATE TABLE `cygj_video_patrol_detail_log` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `CAMERA` varchar(50) DEFAULT NULL,
  `STARTTIME` varchar(20) DEFAULT NULL,
  `ENDTIME` varchar(20) DEFAULT NULL,
  `MEMO` varchar(50) DEFAULT NULL,
  `PATROLLOGID` int(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1209 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_video_patrol_detail_log
-- ----------------------------
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('497', '后门屋檐上', '2016/01/25 12:21:11', '2016/01/25 12:21:17', '打开失败', '320');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('498', '前门屋檐上方', '2016/01/25 12:21:21', '2016/01/25 12:21:31', '打开失败', '321');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('516', '前门屋檐上方', '2016/01/25 16:51:05', '2016/01/25 16:51:14', '打开失败', '332');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('517', '门口2', '2016/01/25 16:51:14', '2016/01/25 16:51:24', '打开失败', '332');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('518', '后门屋檐上', '2016/01/25 16:51:24', '2016/01/25 16:51:34', '打开失败', '332');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('519', '224211', '2016/01/25 16:51:34', '2016/01/25 16:51:45', '打开失败', '332');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('520', '256871', '2016/01/25 16:51:45', '2016/01/25 16:51:59', '打开失败', '332');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('521', '214663', '2016/01/25 16:51:59', '2016/01/25 16:52:08', '打开失败', '332');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('529', '前门屋檐上方', '2016/01/25 17:37:39', '2016/01/25 17:37:48', '打开失败', '335');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('530', '门口2', '2016/01/25 17:37:48', '2016/01/25 17:37:58', '打开失败', '335');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('531', '后门屋檐上', '2016/01/25 17:37:59', '2016/01/25 17:38:08', '打开失败', '335');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('532', '224211', '2016/01/25 17:38:08', '2016/01/25 17:38:21', '打开失败', '335');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('533', '屋檐', '2016/08/29 10:53:35', '2016/08/29 10:53:46', '打开失败', '400');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('534', '门口', '2016/08/29 10:53:47', '2016/08/29 10:53:59', '打开失败', '400');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('535', '后门', '2016/08/29 10:53:59', '2016/08/29 10:54:15', '打开失败', '400');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('536', '屋檐', '2016/08/29 11:05:46', '2016/08/29 11:05:55', '打开失败', '401');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('537', '屋檐', '2016/08/29 11:08:51', '2016/08/29 11:09:02', '打开失败', '402');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('538', '门口', '2016/08/29 11:09:02', '2016/08/29 11:09:13', '打开失败', '402');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('539', '后门', '2016/08/29 11:09:13', '2016/08/29 11:09:33', '打开失败', '402');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('540', '屋檐', '2016/08/29 11:11:10', '2016/08/29 11:11:26', '打开失败', '403');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('541', '屋檐', '2016/08/29 11:11:53', '2016/08/29 11:12:23', '打开失败', '404');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('542', '后门', '2016/08/29 11:13:16', '2016/08/29 11:13:28', '打开失败', '405');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('543', '后门', '2016/08/30 17:00:15', '2016/08/30 17:00:17', '打开失败', '406');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('544', '后门', '2016/08/31 13:53:12', '2016/08/31 13:53:24', '正常', '407');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('545', '门口', '2016/08/31 13:53:24', '2016/08/31 13:53:39', '正常', '407');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('546', '屋檐', '2016/08/31 13:53:39', '2016/08/31 13:53:58', '正常', '407');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('547', '后门', '2016/08/31 14:00:04', '2016/08/31 14:00:13', '正常', '408');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('548', '门口', '2016/08/31 14:00:13', '2016/08/31 14:00:27', '正常', '408');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('549', '屋檐', '2016/08/31 14:00:27', '2016/08/31 14:00:51', '正常', '408');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('550', '后门', '2016/08/31 14:03:45', '2016/08/31 14:04:01', '正常', '409');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('551', '门口', '2016/08/31 14:04:01', '2016/08/31 14:04:18', '正常', '409');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('552', '屋檐', '2016/08/31 14:04:18', '2016/08/31 14:04:53', '正常', '409');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('553', '后门', '2016/08/31 14:09:24', '2016/08/31 14:09:37', '正常', '410');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('554', '门口', '2016/08/31 14:09:38', '2016/08/31 14:09:51', '正常', '410');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('555', '屋檐', '2016/08/31 14:09:51', '2016/08/31 14:10:12', '正常', '410');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('556', '后门', '2016/08/31 14:26:10', '2016/08/31 14:26:20', '正常', '413');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('557', '门口', '2016/08/31 14:26:20', '2016/08/31 14:26:33', '正常', '413');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('558', '屋檐', '2016/08/31 14:26:33', '2016/08/31 14:26:52', '正常', '413');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('559', '后门', '2016/08/31 14:41:18', '2016/08/31 14:41:29', '正常', '414');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('560', '门口', '2016/08/31 14:41:29', '2016/08/31 14:41:42', '正常', '414');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('561', '屋檐', '2016/08/31 14:41:42', '2016/08/31 14:42:29', '正常', '414');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('562', '后门', '2016/08/31 14:48:22', '2016/08/31 14:48:33', '正常', '415');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('563', '门口', '2016/08/31 14:48:33', '2016/08/31 14:48:48', '正常', '415');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('564', '屋檐', '2016/08/31 14:48:48', '2016/08/31 14:49:05', '正常', '415');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('565', '后门', '2016/08/31 15:10:58', '2016/08/31 15:11:08', '正常', '416');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('566', '门口', '2016/08/31 15:11:08', '2016/08/31 15:11:24', '正常', '416');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('567', '屋檐', '2016/08/31 15:11:24', '2016/08/31 15:11:49', '正常', '416');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('568', '后门', '2016/08/31 15:33:42', '2016/08/31 15:33:54', '正常', '417');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('569', '门口', '2016/08/31 15:33:54', '2016/08/31 15:34:07', '正常', '417');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('570', '屋檐', '2016/08/31 15:34:07', '2016/08/31 15:34:24', '正常', '417');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('571', '后门', '2016/08/31 15:37:49', '2016/08/31 15:38:00', '正常', '418');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('572', '门口', '2016/08/31 15:38:00', '2016/08/31 15:38:13', '正常', '418');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('573', '屋檐', '2016/08/31 15:38:13', '2016/08/31 15:38:31', '正常', '418');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('574', '后门', '2016/08/31 15:39:31', '2016/08/31 15:39:48', '正常', '419');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('575', '后门', '2016/08/31 15:43:44', '2016/08/31 15:43:54', '正常', '420');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('576', '门口', '2016/08/31 15:43:54', '2016/08/31 15:44:06', '正常', '420');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('577', '屋檐', '2016/08/31 15:44:06', '2016/08/31 15:44:23', '正常', '420');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('578', '后门', '2016/08/31 16:15:26', '2016/08/31 16:15:36', '打开失败', '421');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('579', '门口', '2016/08/31 16:15:36', '2016/08/31 16:15:48', '正常', '421');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('580', '门口1', '2016/10/11 10:42:59', '2016/10/11 10:43:34', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('581', '屋檐', '2016/10/11 10:43:34', '2016/10/11 10:44:09', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('582', '后门', '2016/10/11 10:44:09', '2016/10/11 10:44:42', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('583', '门口1', '2016/10/11 10:44:42', '2016/10/11 10:45:17', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('584', '后门', '2016/10/11 10:45:17', '2016/10/11 10:45:50', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('585', '屋檐', '2016/10/11 10:45:50', '2016/10/11 10:46:23', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('586', '门口1', '2016/10/11 10:46:23', '2016/10/11 10:46:56', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('587', '后门', '2016/10/11 10:46:56', '2016/10/11 11:03:19', '打开失败', '422');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('588', '门口1', '2016/10/11 11:44:06', '2016/10/11 11:44:39', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('589', '屋檐', '2016/10/11 11:44:40', '2016/10/11 11:45:12', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('590', '后门', '2016/10/11 11:45:12', '2016/10/11 11:45:45', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('591', '门口1', '2016/10/11 11:45:45', '2016/10/11 11:46:18', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('592', '后门', '2016/10/11 11:46:19', '2016/10/11 11:46:52', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('593', '屋檐', '2016/10/11 11:46:52', '2016/10/11 11:47:44', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('594', '门口1', '2016/10/11 11:47:44', '2016/10/11 11:48:22', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('595', '后门', '2016/10/11 11:48:22', '2016/10/11 11:48:55', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('596', '屋檐', '2016/10/11 11:48:55', '2016/10/11 11:49:32', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('597', '门口1', '2016/10/11 11:49:32', '2016/10/11 11:50:05', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('598', '后门', '2016/10/11 11:50:05', '2016/10/11 11:50:09', '打开失败', '423');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('599', '门口1', '2016/10/11 11:50:26', '2016/10/11 11:50:37', '打开失败', '424');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('600', '门口', '2016/10/18 19:49:57', '2016/10/18 19:50:02', '打开失败', '425');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('601', '门口', '2016/10/18 19:50:03', '2016/10/18 19:50:04', '打开失败', '425');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('602', '门口', '2016/10/20 14:11:35', '2016/10/20 14:11:40', '打开失败', '426');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('603', '门口', '2016/10/20 14:14:30', '2016/10/20 14:14:35', '打开失败', '427');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('604', '门口', '2016/10/20 14:14:49', '2016/10/20 14:14:54', '打开失败', '428');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('605', '门口', '2016/10/20 14:19:34', '2016/10/20 14:19:39', '打开失败', '429');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('606', '门口', '2016/10/20 14:25:22', '2016/10/20 14:25:27', '打开失败', '430');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('607', '门口', '2016/10/20 14:26:39', '2016/10/20 14:26:44', '打开失败', '431');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('608', '门口', '2016/10/20 14:26:44', '2016/10/20 14:26:48', '打开失败', '431');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('609', '屋檐', '2016/10/20 14:26:55', '2016/10/20 14:27:00', '打开失败', '432');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('610', '后门', '2016/10/20 14:27:00', '2016/10/20 14:27:05', '打开失败', '432');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('611', '门口', '2016/10/20 14:27:05', '2016/10/20 14:28:13', '打开失败', '432');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('612', '屋檐', '2016/10/20 14:28:15', '2016/10/20 14:28:25', '打开失败', '433');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('613', '后门', '2016/10/20 14:28:25', '2016/10/20 14:28:34', '打开失败', '433');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('614', '门口', '2016/10/20 14:29:37', '2016/10/20 14:29:42', '打开失败', '434');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('615', '门口', '2016/10/20 14:29:42', '2016/10/20 14:29:45', '打开失败', '434');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('616', '屋檐', '2016/10/20 14:29:48', '2016/10/20 14:29:53', '打开失败', '435');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('617', '后门', '2016/10/20 14:29:53', '2016/10/20 14:29:58', '打开失败', '435');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('618', '门口', '2016/10/20 14:29:58', '2016/10/20 14:30:39', '打开失败', '435');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('619', '屋檐', '2016/10/20 14:30:40', '2016/10/20 14:30:49', '打开失败', '436');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('620', '后门', '2016/10/20 14:30:49', '2016/10/20 14:30:57', '打开失败', '436');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('621', '屋檐', '2016/10/20 14:31:58', '2016/10/20 14:32:08', '打开失败', '437');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('622', '屋檐', '2016/10/20 14:33:09', '2016/10/20 14:33:19', '打开失败', '439');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('623', '后门', '2016/10/20 14:33:19', '2016/10/20 14:33:27', '打开失败', '439');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('624', '屋檐', '2016/10/20 14:33:52', '2016/10/20 14:34:01', '打开失败', '440');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('625', '门口', '2016/10/20 14:34:24', '2016/10/20 14:34:30', '打开失败', '441');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('626', '门口', '2016/10/20 15:13:04', '2016/10/20 15:13:10', '打开失败', '442');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('627', '门口', '2016/10/20 15:16:30', '2016/10/20 15:16:35', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('628', '后门', '2016/10/20 15:16:36', '2016/10/20 15:16:43', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('629', '屋檐', '2016/10/20 15:16:43', '2016/10/20 15:16:52', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('630', 'Camera6', '2016/10/20 15:16:52', '2016/10/20 15:17:04', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('631', 'Camera7', '2016/10/20 15:17:04', '2016/10/20 15:17:12', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('632', 'Camera9', '2016/10/20 15:17:12', '2016/10/20 15:17:19', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('633', 'Camera13', '2016/10/20 15:17:19', '2016/10/20 15:17:27', '打开失败', '443');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('634', '门口', '2016/10/20 15:21:35', '2016/10/20 15:21:40', '打开失败', '444');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('635', '门口', '2016/11/08 13:51:20', '2016/11/08 13:51:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('636', '后门', '2016/11/08 13:51:33', '2016/11/08 13:51:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('637', '屋檐', '2016/11/08 13:51:52', '2016/11/08 13:52:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('638', '门口', '2016/11/08 13:53:10', '2016/11/08 13:53:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('639', '后门', '2016/11/08 13:53:23', '2016/11/08 13:53:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('640', '屋檐', '2016/11/08 13:53:42', '2016/11/08 13:54:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('641', 'Camera6', '2016/11/08 13:54:01', '2016/11/08 13:54:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('642', 'Camera7', '2016/11/08 13:54:27', '2016/11/08 13:54:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('643', 'Camera9', '2016/11/08 13:54:49', '2016/11/08 13:55:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('644', 'Camera13', '2016/11/08 13:55:10', '2016/11/08 13:55:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('645', '门口', '2016/11/08 14:17:19', '2016/11/08 14:17:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('646', '后门', '2016/11/08 14:17:32', '2016/11/08 14:17:51', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('647', '屋檐', '2016/11/08 14:17:51', '2016/11/08 14:18:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('648', '门口', '2016/11/08 14:19:48', '2016/11/08 14:20:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('649', '后门', '2016/11/08 14:20:01', '2016/11/08 14:20:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('650', '屋檐', '2016/11/08 14:20:28', '2016/11/08 14:20:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('651', 'Camera6', '2016/11/08 14:20:56', '2016/11/08 14:21:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('652', 'Camera7', '2016/11/08 14:21:28', '2016/11/08 14:21:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('653', 'Camera9', '2016/11/08 14:21:55', '2016/11/08 14:22:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('654', 'Camera13', '2016/11/08 14:22:22', '2016/11/08 14:22:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('655', 'Camera15', '2016/11/08 14:22:51', '2016/11/08 14:23:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('656', 'Camera10', '2016/11/08 14:23:16', '2016/11/08 14:23:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('657', 'Camera17', '2016/11/08 14:23:56', '2016/11/08 14:24:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('658', '门口', '2016/11/09 11:05:31', '2016/11/09 11:05:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('659', '后门', '2016/11/09 11:05:40', '2016/11/09 11:05:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('660', '屋檐', '2016/11/09 11:05:55', '2016/11/09 11:06:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('661', 'Camera6', '2016/11/09 11:06:11', '2016/11/09 11:06:30', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('662', 'Camera7', '2016/11/09 11:06:30', '2016/11/09 11:06:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('663', 'Camera9', '2016/11/09 11:06:43', '2016/11/09 11:06:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('664', 'Camera13', '2016/11/09 11:06:55', '2016/11/09 11:07:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('665', 'Camera15', '2016/11/09 11:07:09', '2016/11/09 11:07:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('666', 'Camera10', '2016/11/09 11:07:29', '2016/11/09 11:07:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('667', 'Camera17', '2016/11/09 11:07:57', '2016/11/09 11:08:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('668', 'Camera8', '2016/11/09 11:08:25', '2016/11/09 11:08:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('669', 'Camera12', '2016/11/09 11:08:47', '2016/11/09 11:09:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('670', 'Camera5', '2016/11/09 11:09:09', '2016/11/09 11:09:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('671', 'Camera14', '2016/11/09 11:09:34', '2016/11/09 11:09:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('672', 'Camera11', '2016/11/09 11:09:49', '2016/11/09 11:10:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('673', 'Camerac16', '2016/11/09 11:10:10', '2016/11/09 11:10:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('674', 'Camera4', '2016/11/09 11:10:39', '2016/11/09 11:10:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('675', '门口', '2016/11/09 11:10:59', '2016/11/09 11:11:17', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('676', '后门', '2016/11/09 11:11:17', '2016/11/09 11:11:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('677', '屋檐', '2016/11/09 11:11:33', '2016/11/09 11:11:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('678', 'Camera6', '2016/11/09 11:11:48', '2016/11/09 11:12:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('679', 'Camera7', '2016/11/09 11:12:08', '2016/11/09 11:12:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('680', 'Camera9', '2016/11/09 11:12:20', '2016/11/09 11:12:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('681', 'Camera13', '2016/11/09 11:12:33', '2016/11/09 11:12:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('682', 'Camera15', '2016/11/09 11:12:46', '2016/11/09 11:13:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('683', 'Camera10', '2016/11/09 11:13:07', '2016/11/09 11:13:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('684', 'Camera17', '2016/11/09 11:13:34', '2016/11/09 11:14:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('685', 'Camera8', '2016/11/09 11:14:03', '2016/11/09 11:14:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('686', 'Camera12', '2016/11/09 11:14:25', '2016/11/09 11:14:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('687', 'Camera5', '2016/11/09 11:14:46', '2016/11/09 11:15:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('688', 'Camera14', '2016/11/09 11:15:11', '2016/11/09 11:15:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('689', 'Camera11', '2016/11/09 11:15:26', '2016/11/09 11:15:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('690', 'Camerac16', '2016/11/09 11:15:46', '2016/11/09 11:16:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('691', 'Camera4', '2016/11/09 11:16:16', '2016/11/09 11:16:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('692', '门口', '2016/11/09 11:20:06', '2016/11/09 11:20:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('693', '后门', '2016/11/09 11:20:15', '2016/11/09 11:20:30', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('694', '屋檐', '2016/11/09 11:20:30', '2016/11/09 11:20:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('695', 'Camera6', '2016/11/09 11:20:46', '2016/11/09 11:21:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('696', 'Camera7', '2016/11/09 11:21:05', '2016/11/09 11:21:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('697', 'Camera9', '2016/11/09 11:21:19', '2016/11/09 11:21:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('698', 'Camera13', '2016/11/09 11:21:32', '2016/11/09 11:21:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('699', 'Camera15', '2016/11/09 11:21:47', '2016/11/09 11:22:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('700', 'Camera10', '2016/11/09 11:22:07', '2016/11/09 11:22:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('701', 'Camera17', '2016/11/09 11:22:36', '2016/11/09 11:23:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('702', 'Camera8', '2016/11/09 11:23:06', '2016/11/09 11:23:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('703', 'Camera12', '2016/11/09 11:23:29', '2016/11/09 11:23:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('704', 'Camera5', '2016/11/09 11:23:50', '2016/11/09 11:24:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('705', 'Camera14', '2016/11/09 11:24:17', '2016/11/09 11:24:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('706', 'Camera11', '2016/11/09 11:24:32', '2016/11/09 11:24:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('707', 'Camerac16', '2016/11/09 11:24:53', '2016/11/09 11:25:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('708', 'Camera4', '2016/11/09 11:25:24', '2016/11/09 11:25:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('709', '门口', '2016/11/09 16:19:53', '2016/11/09 16:20:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('710', '后门', '2016/11/09 16:20:01', '2016/11/09 16:20:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('711', '屋檐', '2016/11/09 16:20:16', '2016/11/09 16:20:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('712', 'Camera6', '2016/11/09 16:20:32', '2016/11/09 16:20:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('713', '门口', '2016/11/09 16:25:18', '2016/11/09 16:25:26', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('714', '后门', '2016/11/09 16:25:26', '2016/11/09 16:25:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('715', '屋檐', '2016/11/09 16:25:44', '2016/11/09 16:26:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('716', 'Camera6', '2016/11/09 16:26:02', '2016/11/09 16:26:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('717', 'Camera7', '2016/11/09 16:26:24', '2016/11/09 16:26:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('718', 'Camera9', '2016/11/09 16:26:41', '2016/11/09 16:26:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('719', 'Camera13', '2016/11/09 16:26:57', '2016/11/09 16:27:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('720', 'Camera15', '2016/11/09 16:27:15', '2016/11/09 16:27:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('721', 'Camera10', '2016/11/09 16:27:38', '2016/11/09 16:28:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('722', 'Camera17', '2016/11/09 16:28:10', '2016/11/09 16:28:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('723', 'Camera8', '2016/11/09 16:28:43', '2016/11/09 16:28:45', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('724', '门口', '2016/11/09 16:30:30', '2016/11/09 16:30:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('725', '后门', '2016/11/09 16:30:38', '2016/11/09 16:30:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('726', '屋檐', '2016/11/09 16:30:56', '2016/11/09 16:31:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('727', 'Camera6', '2016/11/09 16:31:15', '2016/11/09 16:31:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('728', 'Camera7', '2016/11/09 16:31:37', '2016/11/09 16:31:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('729', 'Camera9', '2016/11/09 16:31:53', '2016/11/09 16:32:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('730', 'Camera13', '2016/11/09 16:32:09', '2016/11/09 16:32:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('731', 'Camera15', '2016/11/09 16:32:27', '2016/11/09 16:32:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('732', 'Camera10', '2016/11/09 16:32:50', '2016/11/09 16:33:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('733', 'Camera17', '2016/11/09 16:33:22', '2016/11/09 16:33:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('734', 'Camera8', '2016/11/09 16:33:55', '2016/11/09 16:34:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('735', 'Camera12', '2016/11/09 16:34:21', '2016/11/09 16:34:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('736', 'Camera5', '2016/11/09 16:34:45', '2016/11/09 16:35:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('737', 'Camera14', '2016/11/09 16:35:14', '2016/11/09 16:35:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('738', 'Camera11', '2016/11/09 16:35:31', '2016/11/09 16:35:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('739', 'Camerac16', '2016/11/09 16:35:56', '2016/11/09 16:36:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('740', '门口', '2016/11/09 16:37:59', '2016/11/09 16:38:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('741', '后门', '2016/11/09 16:38:07', '2016/11/09 16:38:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('742', '屋檐', '2016/11/09 16:38:23', '2016/11/09 16:38:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('743', 'Camera6', '2016/11/09 16:38:38', '2016/11/09 16:38:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('744', 'Camera7', '2016/11/09 16:38:58', '2016/11/09 16:39:12', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('745', 'Camera9', '2016/11/09 16:39:12', '2016/11/09 16:39:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('746', 'Camera13', '2016/11/09 16:39:25', '2016/11/09 16:39:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('747', 'Camera15', '2016/11/09 16:39:40', '2016/11/09 16:40:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('748', 'Camera10', '2016/11/09 16:40:00', '2016/11/09 16:40:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('749', 'Camera17', '2016/11/09 16:40:29', '2016/11/09 16:40:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('750', 'Camera8', '2016/11/09 16:40:59', '2016/11/09 16:41:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('751', 'Camera12', '2016/11/09 16:41:22', '2016/11/09 16:41:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('752', 'Camera5', '2016/11/09 16:41:43', '2016/11/09 16:42:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('753', 'Camera14', '2016/11/09 16:42:10', '2016/11/09 16:42:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('754', 'Camera11', '2016/11/09 16:42:25', '2016/11/09 16:42:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('755', 'Camerac16', '2016/11/09 16:42:46', '2016/11/09 16:43:17', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('756', 'Camera4', '2016/11/09 16:43:18', '2016/11/09 17:23:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('757', '门口', '2016/11/09 17:45:51', '2016/11/09 17:45:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('758', '后门', '2016/11/09 17:45:59', '2016/11/09 17:46:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('759', '屋檐', '2016/11/09 17:46:15', '2016/11/09 17:46:30', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('760', 'Camera6', '2016/11/09 17:46:30', '2016/11/09 17:46:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('761', 'Camera7', '2016/11/09 17:46:50', '2016/11/09 17:47:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('762', 'Camera9', '2016/11/09 17:47:04', '2016/11/09 17:47:17', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('763', 'Camera13', '2016/11/09 17:47:17', '2016/11/09 17:47:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('764', 'Camera15', '2016/11/09 17:47:32', '2016/11/09 17:47:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('765', 'Camera10', '2016/11/09 17:47:52', '2016/11/09 17:48:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('766', 'Camera17', '2016/11/09 17:48:21', '2016/11/09 17:48:51', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('767', 'Camera8', '2016/11/09 17:48:52', '2016/11/09 17:49:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('768', 'Camera12', '2016/11/09 17:49:15', '2016/11/09 17:49:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('769', 'Camera5', '2016/11/09 17:49:36', '2016/11/09 17:50:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('770', 'Camera14', '2016/11/09 17:50:02', '2016/11/09 17:50:17', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('771', 'Camera11', '2016/11/09 17:50:17', '2016/11/09 17:50:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('772', 'Camerac16', '2016/11/09 17:50:39', '2016/11/09 17:51:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('773', 'Camera4', '2016/11/09 17:51:10', '2016/11/09 17:51:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('774', '门口', '2016/11/09 17:51:50', '2016/11/09 17:52:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('775', '后门', '2016/11/09 17:52:08', '2016/11/09 17:52:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('776', '门口', '2016/11/09 18:02:41', '2016/11/09 18:02:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('777', '后门', '2016/11/09 18:02:49', '2016/11/09 18:03:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('778', '屋檐', '2016/11/09 18:03:04', '2016/11/09 18:03:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('779', '门口', '2016/11/09 18:06:05', '2016/11/09 18:06:13', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('780', '后门', '2016/11/09 18:06:13', '2016/11/09 18:06:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('781', '屋檐', '2016/11/09 18:06:28', '2016/11/09 18:06:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('782', '门口', '2016/11/09 18:22:34', '2016/11/09 18:22:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('783', '后门', '2016/11/09 18:22:43', '2016/11/09 18:22:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('784', '屋檐', '2016/11/09 18:22:58', '2016/11/09 18:23:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('785', 'Camera6', '2016/11/09 18:23:14', '2016/11/09 18:23:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('786', 'Camera7', '2016/11/09 18:23:33', '2016/11/09 18:23:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('787', 'Camera9', '2016/11/09 18:23:47', '2016/11/09 18:24:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('788', 'Camera13', '2016/11/09 18:24:00', '2016/11/09 18:24:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('789', '门口', '2016/11/09 18:25:44', '2016/11/09 18:25:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('790', '后门', '2016/11/09 18:25:52', '2016/11/09 18:26:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('791', '屋檐', '2016/11/09 18:26:08', '2016/11/09 18:26:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('792', 'Camera6', '2016/11/09 18:26:24', '2016/11/09 18:26:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('793', '门口', '2016/11/09 18:29:11', '2016/11/09 18:29:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('794', '后门', '2016/11/09 18:29:20', '2016/11/09 18:29:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('795', '屋檐', '2016/11/09 18:29:34', '2016/11/09 18:29:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('796', 'Camera6', '2016/11/09 18:29:50', '2016/11/09 18:30:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('797', 'Camera7', '2016/11/09 18:30:10', '2016/11/09 18:30:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('798', 'Camera9', '2016/11/09 18:30:24', '2016/11/09 18:30:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('799', '门口', '2016/11/09 18:38:40', '2016/11/09 18:38:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('800', '后门', '2016/11/09 18:38:49', '2016/11/09 18:38:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('801', '屋檐', '2016/11/09 18:38:55', '2016/11/09 18:39:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('802', 'Camera6', '2016/11/09 18:39:00', '2016/11/09 18:39:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('803', 'Camera7', '2016/11/09 18:39:05', '2016/11/09 18:39:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('804', '门口', '2016/11/09 18:42:04', '2016/11/09 18:42:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('805', '后门', '2016/11/09 18:42:14', '2016/11/09 18:42:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('806', '屋檐', '2016/11/09 18:42:31', '2016/11/09 18:42:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('807', 'Camera6', '2016/11/09 18:42:49', '2016/11/09 18:43:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('808', 'Camera7', '2016/11/09 18:43:10', '2016/11/09 18:43:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('809', 'Camera9', '2016/11/09 18:43:25', '2016/11/09 18:43:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('810', 'Camera13', '2016/11/09 18:43:41', '2016/11/09 18:43:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('811', 'Camera15', '2016/11/09 18:43:57', '2016/11/09 18:44:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('812', 'Camera10', '2016/11/09 18:44:19', '2016/11/09 18:44:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('813', 'Camera17', '2016/11/09 18:44:49', '2016/11/09 18:45:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('814', 'Camera8', '2016/11/09 18:45:21', '2016/11/09 18:45:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('815', 'Camera12', '2016/11/09 18:45:46', '2016/11/09 18:46:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('816', 'Camera5', '2016/11/09 18:46:08', '2016/11/09 18:46:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('817', 'Camera14', '2016/11/09 18:46:36', '2016/11/09 18:46:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('818', 'Camera11', '2016/11/09 18:46:53', '2016/11/09 18:47:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('819', 'Camerac16', '2016/11/09 18:47:16', '2016/11/09 18:47:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('820', 'Camera4', '2016/11/09 18:47:49', '2016/11/09 18:48:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('821', '门口', '2016/11/09 18:52:35', '2016/11/09 18:52:45', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('822', '后门', '2016/11/09 18:52:45', '2016/11/09 18:52:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('823', '门口', '2016/11/09 18:54:31', '2016/11/09 18:54:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('824', '后门', '2016/11/09 18:54:43', '2016/11/09 18:55:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('825', '门口', '2016/11/09 18:56:30', '2016/11/09 18:56:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('826', '后门', '2016/11/09 18:56:43', '2016/11/09 18:56:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('827', '门口', '2016/11/09 19:16:33', '2016/11/09 19:16:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('828', '后门', '2016/11/09 19:16:47', '2016/11/09 19:16:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('829', '屋檐', '2016/11/09 19:16:52', '2016/11/09 19:16:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('830', 'Camera6', '2016/11/09 19:16:57', '2016/11/09 19:17:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('831', 'Camera7', '2016/11/09 19:17:02', '2016/11/09 19:17:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('832', 'Camera9', '2016/11/09 19:17:08', '2016/11/09 19:17:13', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('833', '门口', '2016/11/09 19:18:48', '2016/11/09 19:19:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('834', '后门', '2016/11/09 19:19:01', '2016/11/09 19:19:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('835', '屋檐', '2016/11/09 19:19:06', '2016/11/09 19:19:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('836', 'Camera6', '2016/11/09 19:19:11', '2016/11/09 19:19:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('837', '门口', '2016/11/09 19:21:12', '2016/11/09 19:21:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('838', '后门', '2016/11/09 19:21:25', '2016/11/09 19:21:30', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('839', '屋檐', '2016/11/09 19:21:30', '2016/11/09 19:21:35', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('840', 'Camera6', '2016/11/09 19:21:36', '2016/11/09 19:21:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('841', '门口', '2016/11/09 19:25:24', '2016/11/09 19:25:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('842', '后门', '2016/11/09 19:25:38', '2016/11/09 19:25:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('843', '屋檐', '2016/11/09 19:25:43', '2016/11/09 19:25:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('844', 'Camera6', '2016/11/09 19:25:49', '2016/11/09 19:25:54', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('845', 'Camera7', '2016/11/09 19:25:54', '2016/11/09 19:25:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('846', '门口', '2016/11/09 19:31:13', '2016/11/09 19:31:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('847', '后门', '2016/11/09 19:31:23', '2016/11/09 19:31:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('848', '屋檐', '2016/11/09 19:31:29', '2016/11/09 19:31:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('849', '门口', '2016/11/09 19:32:52', '2016/11/09 19:33:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('850', '后门', '2016/11/09 19:33:02', '2016/11/09 19:33:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('851', '屋檐', '2016/11/09 19:33:19', '2016/11/09 19:33:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('852', 'Camera6', '2016/11/09 19:33:36', '2016/11/09 19:33:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('853', '门口', '2016/11/09 19:35:43', '2016/11/09 19:35:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('854', '后门', '2016/11/09 19:35:53', '2016/11/09 19:36:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('855', '门口', '2016/11/09 19:37:21', '2016/11/09 19:37:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('856', '后门', '2016/11/09 19:37:34', '2016/11/09 19:37:51', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('857', '门口', '2016/11/09 19:38:43', '2016/11/09 19:38:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('858', '后门', '2016/11/09 19:38:56', '2016/11/09 19:39:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('859', '屋檐', '2016/11/09 19:39:02', '2016/11/09 19:39:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('860', 'Camera6', '2016/11/09 19:39:07', '2016/11/09 19:39:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('861', '门口', '2016/11/09 19:40:53', '2016/11/09 19:41:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('862', '后门', '2016/11/09 19:41:05', '2016/11/09 19:41:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('863', '屋檐', '2016/11/09 19:41:22', '2016/11/09 19:41:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('864', 'Camera6', '2016/11/09 19:41:40', '2016/11/09 19:42:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('865', 'Camera7', '2016/11/09 19:42:01', '2016/11/09 19:42:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('866', 'Camera9', '2016/11/09 19:42:16', '2016/11/09 19:42:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('867', 'Camera13', '2016/11/09 19:42:32', '2016/11/09 19:42:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('868', 'Camera15', '2016/11/09 19:42:48', '2016/11/09 19:43:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('869', 'Camera10', '2016/11/09 19:43:10', '2016/11/09 19:43:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('870', '门口', '2016/11/10 10:02:34', '2016/11/10 10:02:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('871', '后门', '2016/11/10 10:02:46', '2016/11/10 10:02:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('872', '屋檐', '2016/11/10 10:02:52', '2016/11/10 10:02:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('873', 'Camera6', '2016/11/10 10:02:57', '2016/11/10 10:03:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('874', '门口', '2016/11/10 10:04:54', '2016/11/10 10:05:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('875', '后门', '2016/11/10 10:05:03', '2016/11/10 10:05:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('876', '门口', '2016/11/10 10:06:09', '2016/11/10 10:06:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('877', '后门', '2016/11/10 10:06:22', '2016/11/10 10:06:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('878', '屋檐', '2016/11/10 10:06:27', '2016/11/10 10:06:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('879', '门口', '2016/11/10 10:07:47', '2016/11/10 10:07:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('880', '后门', '2016/11/10 10:08:00', '2016/11/10 10:08:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('881', '门口', '2016/11/10 10:08:46', '2016/11/10 10:08:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('882', '后门', '2016/11/10 10:08:56', '2016/11/10 10:09:13', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('883', '门口', '2016/11/10 10:10:21', '2016/11/10 10:10:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('884', '后门', '2016/11/10 10:10:34', '2016/11/10 10:10:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('885', '屋檐', '2016/11/10 10:10:40', '2016/11/10 10:10:45', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('886', '门口', '2016/11/10 10:12:49', '2016/11/10 10:12:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('887', '后门', '2016/11/10 10:12:59', '2016/11/10 10:13:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('888', '屋檐', '2016/11/10 10:13:16', '2016/11/10 10:13:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('889', 'Camera6', '2016/11/10 10:13:33', '2016/11/10 10:13:54', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('890', 'Camera7', '2016/11/10 10:13:54', '2016/11/10 10:14:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('891', 'Camera9', '2016/11/10 10:14:10', '2016/11/10 10:14:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('892', 'Camera13', '2016/11/10 10:14:25', '2016/11/10 10:14:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('893', 'Camera15', '2016/11/10 10:14:41', '2016/11/10 10:15:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('894', 'Camera10', '2016/11/10 10:15:03', '2016/11/10 10:15:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('895', 'Camera17', '2016/11/10 10:15:33', '2016/11/10 10:16:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('896', 'Camera8', '2016/11/10 10:16:05', '2016/11/10 10:16:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('897', 'Camera12', '2016/11/10 10:16:29', '2016/11/10 10:16:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('898', 'Camera5', '2016/11/10 10:16:52', '2016/11/10 10:17:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('899', 'Camera14', '2016/11/10 10:17:20', '2016/11/10 10:17:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('900', 'Camera11', '2016/11/10 10:17:36', '2016/11/10 10:18:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('901', 'Camerac16', '2016/11/10 10:18:00', '2016/11/10 10:18:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('902', 'Camera4', '2016/11/10 10:18:32', '2016/11/10 10:19:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('903', '门口', '2016/11/10 10:19:09', '2016/11/10 10:19:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('904', '后门', '2016/11/10 10:19:28', '2016/11/10 10:19:45', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('905', '屋檐', '2016/11/10 10:19:45', '2016/11/10 10:20:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('906', 'Camera6', '2016/11/10 10:20:02', '2016/11/10 10:20:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('907', 'Camera7', '2016/11/10 10:20:23', '2016/11/10 10:20:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('908', 'Camera9', '2016/11/10 10:20:39', '2016/11/10 10:20:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('909', '门口', '2016/11/10 10:37:22', '2016/11/10 10:37:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('910', '后门', '2016/11/10 10:37:32', '2016/11/10 10:37:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('911', '门口', '2016/11/10 10:39:02', '2016/11/10 10:39:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('912', '后门', '2016/11/10 10:39:15', '2016/11/10 10:39:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('913', '屋檐', '2016/11/10 10:39:32', '2016/11/10 10:39:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('914', '门口', '2016/11/10 11:21:53', '2016/11/10 11:22:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('915', '后门', '2016/11/10 11:22:02', '2016/11/10 11:22:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('916', '屋檐', '2016/11/10 11:22:11', '2016/11/10 11:22:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('917', 'Camera6', '2016/11/10 11:22:16', '2016/11/10 11:22:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('918', 'Camera7', '2016/11/10 11:22:21', '2016/11/10 11:22:26', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('919', 'Camera9', '2016/11/10 11:22:27', '2016/11/10 11:22:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('920', 'Camera13', '2016/11/10 11:22:34', '2016/11/10 11:22:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('921', 'Camera15', '2016/11/10 11:22:39', '2016/11/10 11:22:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('922', '门口', '2016/11/10 11:25:34', '2016/11/10 11:25:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('923', '后门', '2016/11/10 11:25:45', '2016/11/10 11:25:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('924', '屋檐', '2016/11/10 11:25:54', '2016/11/10 11:25:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('925', 'Camera6', '2016/11/10 11:25:59', '2016/11/10 11:26:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('926', 'Camera7', '2016/11/10 11:26:04', '2016/11/10 11:26:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('927', 'Camera9', '2016/11/10 11:26:12', '2016/11/10 11:26:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('928', 'Camera13', '2016/11/10 11:26:18', '2016/11/10 11:26:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('929', '门口', '2016/11/10 11:30:11', '2016/11/10 11:30:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('930', '后门', '2016/11/10 11:30:22', '2016/11/10 11:30:35', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('931', '屋檐', '2016/11/10 11:30:35', '2016/11/10 11:30:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('932', 'Camera6', '2016/11/10 11:30:49', '2016/11/10 11:31:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('933', '门口', '2016/11/10 11:33:45', '2016/11/10 11:33:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('934', '后门', '2016/11/10 11:33:56', '2016/11/10 11:34:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('935', '屋檐', '2016/11/10 11:34:09', '2016/11/10 11:34:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('936', 'Camera6', '2016/11/10 11:34:23', '2016/11/10 11:34:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('937', 'Camera7', '2016/11/10 11:34:40', '2016/11/10 11:34:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('938', 'Camera9', '2016/11/10 11:34:52', '2016/11/10 11:35:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('939', '门口', '2016/11/10 11:38:34', '2016/11/10 11:38:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('940', '后门', '2016/11/10 11:38:44', '2016/11/10 11:38:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('941', '屋檐', '2016/11/10 11:38:58', '2016/11/10 11:39:12', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('942', 'Camera6', '2016/11/10 11:39:12', '2016/11/10 11:39:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('943', 'Camera7', '2016/11/10 11:39:29', '2016/11/10 11:39:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('944', 'Camera9', '2016/11/10 11:39:41', '2016/11/10 11:39:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('945', 'Camera13', '2016/11/10 11:39:53', '2016/11/10 11:40:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('946', '门口', '2016/11/10 11:45:19', '2016/11/10 11:45:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('947', '后门', '2016/11/10 11:45:29', '2016/11/10 11:45:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('948', '屋檐', '2016/11/10 11:45:43', '2016/11/10 11:45:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('949', '门口', '2016/11/10 13:06:58', '2016/11/10 13:07:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('950', '后门', '2016/11/10 13:07:09', '2016/11/10 13:07:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('951', '屋檐', '2016/11/10 13:07:22', '2016/11/10 13:07:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('952', 'Camera6', '2016/11/10 13:07:36', '2016/11/10 13:07:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('953', 'Camera7', '2016/11/10 13:07:53', '2016/11/10 13:08:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('954', 'Camera9', '2016/11/10 13:08:05', '2016/11/10 13:08:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('955', '门口', '2016/11/10 13:08:57', '2016/11/10 13:09:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('956', '后门', '2016/11/10 13:09:07', '2016/11/10 13:09:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('957', '屋檐', '2016/11/10 13:09:20', '2016/11/10 13:09:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('958', 'Camera6', '2016/11/10 13:09:33', '2016/11/10 13:09:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('959', 'Camera7', '2016/11/10 13:09:49', '2016/11/10 13:09:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('960', 'Camera9', '2016/11/10 13:09:59', '2016/11/10 13:10:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('961', '门口', '2016/11/10 13:14:36', '2016/11/10 13:14:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('962', '后门', '2016/11/10 13:14:53', '2016/11/10 13:15:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('963', '屋檐', '2016/11/10 13:15:15', '2016/11/10 13:15:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('964', 'Camera6', '2016/11/10 13:15:37', '2016/11/10 13:16:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('965', 'Camera7', '2016/11/10 13:16:03', '2016/11/10 13:16:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('966', 'Camera9', '2016/11/10 13:16:25', '2016/11/10 13:16:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('967', 'Camera13', '2016/11/10 13:16:46', '2016/11/10 13:17:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('968', 'Camera15', '2016/11/10 13:17:09', '2016/11/10 13:17:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('969', 'Camera10', '2016/11/10 13:17:28', '2016/11/10 13:18:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('970', 'Camera17', '2016/11/10 13:18:02', '2016/11/10 13:18:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('971', 'Camera8', '2016/11/10 13:18:37', '2016/11/10 13:19:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('972', 'Camera12', '2016/11/10 13:19:06', '2016/11/10 13:19:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('973', 'Camera5', '2016/11/10 13:19:25', '2016/11/10 13:19:57', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('974', '门口', '2016/11/10 13:21:18', '2016/11/10 13:21:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('975', '后门', '2016/11/10 13:21:28', '2016/11/10 13:21:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('976', '屋檐', '2016/11/10 13:21:42', '2016/11/10 13:21:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('977', '门口', '2016/11/10 13:27:59', '2016/11/10 13:28:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('978', '后门', '2016/11/10 13:28:09', '2016/11/10 13:28:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('979', '屋檐', '2016/11/10 13:28:23', '2016/11/10 13:28:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('980', 'Camera6', '2016/11/10 13:28:37', '2016/11/10 13:28:54', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('981', '门口', '2016/11/10 13:48:41', '2016/11/10 13:48:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('982', '后门', '2016/11/10 13:48:51', '2016/11/10 13:49:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('983', '屋檐', '2016/11/10 13:49:04', '2016/11/10 13:49:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('984', 'Camera6', '2016/11/10 13:49:21', '2016/11/10 13:49:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('985', '门口', '2016/11/10 13:50:40', '2016/11/10 13:50:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('986', '后门', '2016/11/10 13:50:50', '2016/11/10 13:51:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('987', '门口', '2016/11/10 13:52:53', '2016/11/10 13:53:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('988', '后门', '2016/11/10 13:53:03', '2016/11/10 13:53:17', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('989', '屋檐', '2016/11/10 13:53:17', '2016/11/10 13:53:35', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('990', 'Camera6', '2016/11/10 13:53:35', '2016/11/10 13:53:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('991', 'Camera7', '2016/11/10 13:53:53', '2016/11/10 13:54:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('992', '门口', '2016/11/10 14:04:55', '2016/11/10 14:05:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('993', '门口', '2016/11/10 14:06:05', '2016/11/10 14:06:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('994', '门口', '2016/11/10 14:08:23', '2016/11/10 14:08:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('995', '后门', '2016/11/10 14:08:33', '2016/11/10 14:08:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('996', '屋檐', '2016/11/10 14:08:54', '2016/11/10 14:09:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('997', 'Camera6', '2016/11/10 14:09:11', '2016/11/10 14:09:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('998', 'Camera7', '2016/11/10 14:09:32', '2016/11/10 14:09:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('999', 'Camera9', '2016/11/10 14:09:48', '2016/11/10 14:10:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1000', 'Camera13', '2016/11/10 14:10:05', '2016/11/10 14:10:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1001', '门口', '2016/11/10 14:11:59', '2016/11/10 14:12:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1002', '后门', '2016/11/10 14:12:11', '2016/11/10 14:12:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1003', '屋檐', '2016/11/10 14:12:31', '2016/11/10 14:12:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1004', 'Camera6', '2016/11/10 14:12:49', '2016/11/10 14:13:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1005', '门口', '2016/11/10 14:14:05', '2016/11/10 14:14:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1006', '后门', '2016/11/10 14:14:18', '2016/11/10 14:14:35', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1007', '屋檐', '2016/11/10 14:14:35', '2016/11/10 14:14:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1008', 'Camera6', '2016/11/10 14:14:53', '2016/11/10 14:15:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1009', 'Camera7', '2016/11/10 14:15:15', '2016/11/10 14:15:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1010', 'Camera9', '2016/11/10 14:15:31', '2016/11/10 14:15:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1011', 'Camera13', '2016/11/10 14:15:47', '2016/11/10 14:16:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1012', 'Camera15', '2016/11/10 14:16:02', '2016/11/10 14:16:33', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1013', '门口', '2016/11/10 14:18:09', '2016/11/10 14:18:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1014', '后门', '2016/11/10 14:18:22', '2016/11/10 14:18:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1015', '屋檐', '2016/11/10 14:18:42', '2016/11/10 14:18:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1016', 'Camera6', '2016/11/10 14:18:59', '2016/11/10 14:19:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1017', 'Camera7', '2016/11/10 14:19:21', '2016/11/10 14:19:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1018', 'Camera9', '2016/11/10 14:19:37', '2016/11/10 14:19:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1019', 'Camera13', '2016/11/10 14:19:54', '2016/11/10 14:20:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1020', 'Camera15', '2016/11/10 14:20:11', '2016/11/10 14:20:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1021', 'Camera10', '2016/11/10 14:20:38', '2016/11/10 14:21:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1022', 'Camera17', '2016/11/10 14:21:08', '2016/11/10 14:21:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1023', 'Camera8', '2016/11/10 14:21:41', '2016/11/10 14:22:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1024', 'Camera12', '2016/11/10 14:22:06', '2016/11/10 14:22:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1025', 'Camera5', '2016/11/10 14:22:34', '2016/11/10 14:23:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1026', 'Camera14', '2016/11/10 14:23:02', '2016/11/10 14:23:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1027', 'Camera11', '2016/11/10 14:23:23', '2016/11/10 14:23:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1028', 'Camerac16', '2016/11/10 14:23:47', '2016/11/10 14:24:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1029', '门口', '2016/11/10 18:57:56', '2016/11/10 18:58:09', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1030', '后门', '2016/11/10 18:58:09', '2016/11/10 18:58:30', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1031', '屋檐', '2016/11/10 18:58:30', '2016/11/10 18:58:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1032', 'Camera6', '2016/11/10 18:58:47', '2016/11/10 18:59:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1033', 'Camera7', '2016/11/10 18:59:09', '2016/11/10 18:59:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1034', 'Camera9', '2016/11/10 18:59:24', '2016/11/10 18:59:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1035', 'Camera13', '2016/11/10 18:59:41', '2016/11/10 18:59:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1036', 'Camera15', '2016/11/10 18:59:59', '2016/11/10 19:00:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1037', 'Camera10', '2016/11/10 19:00:26', '2016/11/10 19:00:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1038', '门口', '2016/11/10 19:04:16', '2016/11/10 19:04:30', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1039', '后门', '2016/11/10 19:04:30', '2016/11/10 19:04:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1040', '屋檐', '2016/11/10 19:04:52', '2016/11/10 19:05:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1041', 'Camera6', '2016/11/10 19:05:11', '2016/11/10 19:05:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1042', '门口', '2016/11/10 19:06:58', '2016/11/10 19:07:12', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1043', '后门', '2016/11/10 19:07:12', '2016/11/10 19:07:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1044', '屋檐', '2016/11/10 19:07:34', '2016/11/10 19:07:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1045', 'Camera6', '2016/11/10 19:07:53', '2016/11/10 19:08:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1046', '门口', '2016/11/10 19:08:45', '2016/11/10 19:09:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1047', '后门', '2016/11/10 19:09:00', '2016/11/10 19:09:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1048', '屋檐', '2016/11/10 19:09:24', '2016/11/10 19:09:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1049', 'Camera6', '2016/11/10 19:09:43', '2016/11/10 19:10:08', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1050', 'Camera7', '2016/11/10 19:10:08', '2016/11/10 19:10:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1051', 'Camera9', '2016/11/10 19:10:25', '2016/11/10 19:10:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1052', 'Camera13', '2016/11/10 19:10:42', '2016/11/10 19:11:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1053', 'Camera15', '2016/11/10 19:11:03', '2016/11/10 19:11:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1054', '门口', '2016/11/11 09:58:52', '2016/11/11 09:59:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1055', '后门', '2016/11/11 09:59:07', '2016/11/11 09:59:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1056', '屋檐', '2016/11/11 09:59:31', '2016/11/11 09:59:50', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1057', 'Camera6', '2016/11/11 09:59:50', '2016/11/11 10:00:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1058', 'Camera7', '2016/11/11 10:00:15', '2016/11/11 10:00:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1059', 'Camera9', '2016/11/11 10:00:32', '2016/11/11 10:00:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1060', 'Camera13', '2016/11/11 10:00:49', '2016/11/11 10:01:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1061', 'Camera15', '2016/11/11 10:01:10', '2016/11/11 10:01:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1062', 'Camera10', '2016/11/11 10:01:39', '2016/11/11 10:02:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1063', '门口', '2016/11/11 10:19:15', '2016/11/11 10:19:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1064', '后门', '2016/11/11 10:19:29', '2016/11/11 10:19:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1065', '屋檐', '2016/11/11 10:19:52', '2016/11/11 10:20:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1066', 'Camera6', '2016/11/11 10:20:11', '2016/11/11 10:20:35', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1067', 'Camera7', '2016/11/11 10:20:35', '2016/11/11 10:20:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1068', 'Camera9', '2016/11/11 10:20:52', '2016/11/11 10:21:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1069', 'Camera13', '2016/11/11 10:21:10', '2016/11/11 10:21:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1070', 'Camera15', '2016/11/11 10:21:29', '2016/11/11 10:21:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1071', '门口', '2016/11/11 10:45:00', '2016/11/11 10:45:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1072', '后门', '2016/11/11 10:45:14', '2016/11/11 10:45:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1073', '屋檐', '2016/11/11 10:45:37', '2016/11/11 10:45:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1074', 'Camera6', '2016/11/11 10:45:56', '2016/11/11 10:46:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1075', '门口', '2016/11/11 10:47:05', '2016/11/11 10:47:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1076', '后门', '2016/11/11 10:47:19', '2016/11/11 10:47:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1077', '屋檐', '2016/11/11 10:47:41', '2016/11/11 10:48:00', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1078', 'Camera6', '2016/11/11 10:48:00', '2016/11/11 10:48:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1079', 'Camera7', '2016/11/11 10:48:23', '2016/11/11 10:48:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1080', 'Camera9', '2016/11/11 10:48:40', '2016/11/11 10:48:59', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1081', 'Camera13', '2016/11/11 10:48:59', '2016/11/11 10:49:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1082', 'Camera15', '2016/11/11 10:49:18', '2016/11/11 10:49:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1083', 'Camera10', '2016/11/11 10:49:47', '2016/11/11 10:50:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1084', 'Camera17', '2016/11/11 10:50:19', '2016/11/11 10:50:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1085', 'Camera8', '2016/11/11 10:50:53', '2016/11/11 10:51:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1086', 'Camera12', '2016/11/11 10:51:20', '2016/11/11 10:51:49', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1087', 'Camera5', '2016/11/11 10:51:49', '2016/11/11 10:52:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1088', 'Camera14', '2016/11/11 10:52:19', '2016/11/11 10:52:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1089', 'Camera11', '2016/11/11 10:52:41', '2016/11/11 10:53:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1090', 'Camerac16', '2016/11/11 10:53:07', '2016/11/11 10:53:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1091', '门口', '2016/11/11 13:27:26', '2016/11/11 13:27:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1092', '后门', '2016/11/11 13:27:40', '2016/11/11 13:28:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1093', '屋檐', '2016/11/11 13:28:03', '2016/11/11 13:28:22', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1094', 'Camera6', '2016/11/11 13:28:22', '2016/11/11 13:28:45', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1095', 'Camera7', '2016/11/11 13:28:45', '2016/11/11 13:29:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1096', '门口', '2016/11/11 13:29:44', '2016/11/11 13:29:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1097', '后门', '2016/11/11 13:29:58', '2016/11/11 13:30:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1098', '屋檐', '2016/11/11 13:30:20', '2016/11/11 13:30:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1099', 'Camera6', '2016/11/11 13:30:39', '2016/11/11 13:31:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1100', 'Camera7', '2016/11/11 13:31:01', '2016/11/11 13:31:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1101', 'Camera9', '2016/11/11 13:31:19', '2016/11/11 13:31:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1102', 'Camera13', '2016/11/11 13:31:37', '2016/11/11 13:31:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1103', 'Camera15', '2016/11/11 13:31:55', '2016/11/11 13:32:23', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1104', 'Camera10', '2016/11/11 13:32:23', '2016/11/11 13:32:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1105', 'Camera17', '2016/11/11 13:32:55', '2016/11/11 13:33:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1106', 'Camera8', '2016/11/11 13:33:29', '2016/11/11 13:33:55', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1107', 'Camera12', '2016/11/11 13:33:56', '2016/11/11 13:34:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1108', 'Camera5', '2016/11/11 13:34:24', '2016/11/11 13:34:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1109', 'Camera14', '2016/11/11 13:34:53', '2016/11/11 13:35:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1110', 'Camera11', '2016/11/11 13:35:15', '2016/11/11 13:35:40', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1111', '门口', '2016/11/11 16:55:53', '2016/11/11 16:56:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1112', '后门', '2016/11/11 16:56:07', '2016/11/11 16:56:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1113', '屋檐', '2016/11/11 16:56:29', '2016/11/11 16:56:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1114', 'Camera6', '2016/11/11 16:56:48', '2016/11/11 16:57:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1115', 'Camera7', '2016/11/11 16:57:11', '2016/11/11 16:57:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1116', 'Camera9', '2016/11/11 16:57:28', '2016/11/11 16:57:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1117', 'Camera13', '2016/11/11 16:57:46', '2016/11/11 16:58:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1118', 'Camera15', '2016/11/11 16:58:05', '2016/11/11 16:58:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1119', '门口', '2016/11/18 13:55:22', '2016/11/18 13:55:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1120', '后门', '2016/11/18 13:55:37', '2016/11/18 13:55:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1121', '门口', '2016/11/18 13:56:01', '2016/11/18 13:56:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1122', '后门', '2016/11/18 13:56:06', '2016/11/18 13:56:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1123', '屋檐', '2016/11/18 13:56:18', '2016/11/18 13:56:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1124', '门口', '2016/11/18 13:56:43', '2016/11/18 13:56:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1125', '后门', '2016/11/18 13:56:49', '2016/11/18 13:57:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1126', '屋檐', '2016/11/18 13:57:01', '2016/11/18 13:57:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1127', '门口', '2016/11/18 13:59:12', '2016/11/18 13:59:26', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1128', '后门', '2016/11/18 13:59:26', '2016/11/18 13:59:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1129', '门口', '2016/11/18 14:01:45', '2016/11/18 14:02:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1130', '后门', '2016/11/18 14:02:02', '2016/11/18 14:02:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1131', '门口', '2016/11/18 14:02:41', '2016/11/18 14:02:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1132', '后门', '2016/11/18 14:02:46', '2016/11/18 14:03:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1133', '屋檐', '2016/11/18 14:03:02', '2016/11/18 14:03:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1134', '门口', '2016/11/18 14:13:24', '2016/11/18 14:13:38', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1135', '后门', '2016/11/18 14:13:38', '2016/11/18 14:14:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1136', '屋檐', '2016/11/18 14:14:01', '2016/11/18 14:14:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1137', '门口', '2016/11/18 14:14:21', '2016/11/18 14:14:26', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1138', '后门', '2016/11/18 14:14:26', '2016/11/18 14:14:37', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1139', '屋檐', '2016/11/18 14:14:37', '2016/11/18 14:14:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1140', '门口', '2016/11/18 14:14:57', '2016/11/18 14:15:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1141', '后门', '2016/11/18 14:15:03', '2016/11/18 14:15:16', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1142', '屋檐', '2016/11/18 14:15:16', '2016/11/18 14:15:32', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1143', 'Camera6', '2016/11/18 14:15:32', '2016/11/18 14:15:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1144', '门口', '2016/11/18 14:15:57', '2016/11/18 14:16:03', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1145', '后门', '2016/11/18 14:16:03', '2016/11/18 14:16:14', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1146', '屋檐', '2016/11/18 14:16:14', '2016/11/18 14:16:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1147', '门口', '2016/11/18 14:16:42', '2016/11/18 14:16:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1148', '后门', '2016/11/18 14:16:56', '2016/11/18 14:17:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1149', '屋檐', '2016/11/18 14:17:18', '2016/11/18 14:17:21', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1150', '门口', '2016/11/18 14:17:38', '2016/11/18 14:17:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1151', '后门', '2016/11/18 14:17:43', '2016/11/18 14:17:54', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1152', '屋檐', '2016/11/18 14:17:54', '2016/11/18 14:17:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1153', '门口', '2016/11/18 14:18:15', '2016/11/18 14:18:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1154', '门口', '2016/11/18 14:18:37', '2016/11/18 14:18:42', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1155', '后门', '2016/11/18 14:18:42', '2016/11/18 14:18:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1156', '屋檐', '2016/11/18 14:18:53', '2016/11/18 14:18:58', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1157', '门口', '2016/11/18 14:22:52', '2016/11/18 14:23:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1158', '后门', '2016/11/18 14:23:06', '2016/11/18 14:23:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1159', '门口', '2016/11/18 14:23:36', '2016/11/18 14:23:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1160', '后门', '2016/11/18 14:23:41', '2016/11/18 14:23:54', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1161', '屋檐', '2016/11/18 14:23:54', '2016/11/18 14:24:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1162', 'Camera6', '2016/11/18 14:24:10', '2016/11/18 14:24:10', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1163', '门口', '2016/11/18 14:25:12', '2016/11/18 14:25:17', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1164', '后门', '2016/11/18 14:25:17', '2016/11/18 14:25:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1165', '屋檐', '2016/11/18 14:25:29', '2016/11/18 14:25:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1166', '门口', '2016/11/18 14:27:32', '2016/11/18 14:27:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1167', '后门', '2016/11/18 14:27:52', '2016/11/18 14:28:04', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1168', '门口', '2016/11/18 14:28:18', '2016/11/18 14:28:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1169', '后门', '2016/11/18 14:28:40', '2016/11/18 14:29:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1170', '屋檐', '2016/11/18 14:29:25', '2016/11/18 14:29:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1171', '门口', '2016/11/18 14:32:29', '2016/11/18 14:32:43', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1172', '后门', '2016/11/18 14:32:43', '2016/11/18 14:32:52', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1173', '屋檐', '2016/11/18 14:33:05', '2016/11/18 14:33:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1174', '门口', '2016/11/18 14:44:00', '2016/11/18 14:44:02', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1175', '门口', '2016/11/18 14:45:07', '2016/11/18 14:45:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1176', '门口', '2016/11/18 14:46:13', '2016/11/18 14:46:27', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1177', '后门', '2016/11/18 14:46:27', '2016/11/18 14:46:36', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1178', '门口', '2016/11/18 14:46:53', '2016/11/18 14:47:13', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1179', '后门', '2016/11/18 14:47:13', '2016/11/18 14:47:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1180', '门口', '2016/11/18 15:50:47', '2016/11/18 15:51:01', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1181', '门口', '2016/11/18 15:52:08', '2016/11/18 15:52:19', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1182', '门口', '2016/11/18 15:53:26', '2016/11/18 15:53:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1183', '门口', '2016/11/18 15:54:57', '2016/11/18 15:55:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1184', '门口', '2016/11/18 16:00:11', '2016/11/18 16:00:25', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1185', '后门', '2016/11/18 16:00:25', '2016/11/18 16:00:48', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1186', '屋檐', '2016/11/18 16:00:48', '2016/11/18 16:01:07', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1187', '门口', '2016/11/18 16:39:17', '2016/11/18 16:39:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1188', '门口', '2016/11/18 16:45:04', '2016/11/18 16:45:18', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1189', '后门', '2016/11/18 16:45:18', '2016/11/18 16:45:44', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1190', '门口', '2016/11/18 16:48:20', '2016/11/18 16:48:35', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1191', '门口', '2016/11/18 16:51:17', '2016/11/18 16:51:31', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1192', '后门', '2016/11/18 16:51:31', '2016/11/18 16:51:53', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1193', '门口', '2016/11/18 16:54:20', '2016/11/18 16:54:34', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1194', '后门', '2016/11/18 16:54:34', '2016/11/18 16:54:56', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1195', '屋檐', '2016/11/18 16:54:56', '2016/11/18 16:55:15', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1196', 'Camera6', '2016/11/18 16:55:16', '2016/11/18 16:55:39', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1197', '门口', '2016/11/18 17:01:52', '2016/11/18 17:02:06', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1198', '后门', '2016/11/18 17:02:06', '2016/11/18 17:02:28', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1199', '屋檐', '2016/11/18 17:02:28', '2016/11/18 17:02:47', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1200', 'Camera6', '2016/11/18 17:02:47', '2016/11/18 17:03:11', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1201', '门口', '2016/11/18 17:17:10', '2016/11/18 17:17:24', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1202', '后门', '2016/11/18 17:17:24', '2016/11/18 17:17:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1203', '屋檐', '2016/11/18 17:17:46', '2016/11/18 17:18:05', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1204', 'Camera6', '2016/11/18 17:18:06', '2016/11/18 17:18:29', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1205', 'Camera7', '2016/11/18 17:18:29', '2016/11/18 17:18:46', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1206', '门口', '2016/11/18 17:20:06', '2016/11/18 17:20:20', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1207', '后门', '2016/11/18 17:20:20', '2016/11/18 17:20:41', '打开失败', '0');
INSERT INTO `cygj_video_patrol_detail_log` VALUES ('1208', '屋檐', '2016/11/18 17:20:41', '2016/11/18 17:21:01', '打开失败', '0');

-- ----------------------------
-- Table structure for `cygj_video_patrol_log`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_video_patrol_log`;
CREATE TABLE `cygj_video_patrol_log` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `PERSON` varchar(10) DEFAULT NULL,
  `STARTDATE` varchar(20) DEFAULT NULL,
  `PLANNAME` varchar(40) DEFAULT NULL,
  `EXCEPTION` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=572 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_video_patrol_log
-- ----------------------------
INSERT INTO `cygj_video_patrol_log` VALUES ('313', '管理员', '2016/01/25 10:16:26', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('315', '管理员', '2016/01/25 10:31:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('316', '管理员', '2016/01/25 10:35:03', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('317', '管理员', '2016/01/25 12:16:30', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('318', '管理员', '2016/01/25 12:17:42', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('319', '管理员', '2016/01/25 12:20:34', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('320', '管理员', '2016/01/25 12:20:41', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('321', '管理员', '2016/01/25 12:21:21', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('323', '管理员', '2016/01/25 12:30:36', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('324', '管理员', '2016/01/25 12:30:50', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('325', '管理员', '2016/01/25 13:43:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('327', null, '2016/01/25 16:34:14', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('329', '管理员', '2016/01/25 16:45:28', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('331', '管理员', '2016/01/25 16:49:37', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('332', '管理员', '2016/01/25 16:51:05', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('333', '管理员', '2016/01/25 17:35:50', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('335', '管理员', '2016/01/25 17:37:39', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('337', '管理员', '2016/01/25 17:42:09', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('339', '管理员', '2016/01/25 17:56:11', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('341', '管理员', '2016/01/25 17:59:44', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('343', '管理员', '2016/01/26 10:04:09', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('345', '管理员', '2016/01/26 10:28:54', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('347', '管理员', '2016/01/26 10:44:29', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('349', null, '2016/01/26 11:34:32', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('351', null, '2016/01/26 13:57:07', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('353', null, '2016/01/26 13:59:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('355', null, '2016/01/26 14:01:44', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('357', '管理员', '2016/01/27 09:54:49', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('358', '管理员', '2016/01/27 10:00:19', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('359', '管理员', '2016/01/27 10:00:56', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('361', '管理员', '2016/01/27 10:10:12', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('363', '管理员', '2016/02/29 09:23:31', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('365', '管理员', '2016/03/07 10:07:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('367', '管理员', '2016/03/21 16:28:30', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('369', '管理员', '2016/03/21 16:56:36', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('371', '管理员', '2016/07/12 09:56:31', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('373', '管理员', '2016/07/12 10:52:54', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('374', '管理员', '2016/07/12 10:58:58', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('375', '管理员', '2016/07/12 11:10:36', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('376', '管理员', '2016/07/12 11:11:50', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('377', 'test', '2016/07/12 11:36:37', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('379', 'test', '2016/07/12 13:33:36', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('380', '管理员', '2016/07/12 13:38:07', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('381', '管理员', '2016/07/12 13:40:22', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('383', '管理员', '2016/07/14 09:16:36', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('384', '管理员', '2016/07/14 09:17:17', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('385', '管理员', '2016/07/14 14:41:38', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('387', 'test', '2016/07/14 14:59:27', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('389', '管理员', '2016/07/27 10:59:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('390', '管理员', '2016/07/27 11:01:11', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('391', '管理员', '2016/07/29 13:57:16', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('393', '管理员', '2016/07/29 14:15:38', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('394', '管理员', '2016/07/29 14:16:40', '新建方案3', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('395', '管理员', '2016/07/29 14:16:59', '新建方案3', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('396', '管理员', '2016/07/29 14:18:19', '新建方案3', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('397', '管理员', '2016/07/29 14:23:19', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('398', '管理员', '2016/08/26 17:00:23', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('399', '管理员', '2016/08/29 10:50:23', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('400', '管理员', '2016/08/29 10:53:35', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('401', '管理员', '2016/08/29 11:05:46', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('402', '管理员', '2016/08/29 11:08:51', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('403', '管理员', '2016/08/29 11:11:10', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('404', '管理员', '2016/08/29 11:11:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('405', '管理员', '2016/08/29 11:13:16', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('406', '管理员', '2016/08/30 17:00:15', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('407', '管理员', '2016/08/31 13:53:13', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('408', '管理员', '2016/08/31 14:00:05', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('409', '管理员', '2016/08/31 14:03:46', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('410', '管理员', '2016/08/31 14:09:25', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('411', '管理员', '2016/08/31 14:22:06', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('412', '管理员', '2016/08/31 14:23:35', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('413', '管理员', '2016/08/31 14:26:11', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('414', '管理员', '2016/08/31 14:41:19', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('415', '管理员', '2016/08/31 14:48:24', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('416', '管理员', '2016/08/31 15:10:59', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('417', '管理员', '2016/08/31 15:33:43', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('418', '管理员', '2016/08/31 15:37:50', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('419', '管理员', '2016/08/31 15:39:32', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('420', '管理员', '2016/08/31 15:43:45', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('421', '管理员', '2016/08/31 16:15:28', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('422', '管理员', '2016/10/11 10:43:02', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('423', '管理员', '2016/10/11 11:44:09', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('424', '管理员', '2016/10/11 11:50:29', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('425', '', '2016/10/18 19:49:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('426', '', '2016/10/20 14:11:35', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('427', '', '2016/10/20 14:14:30', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('428', '', '2016/10/20 14:14:49', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('429', '', '2016/10/20 14:19:34', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('430', '', '2016/10/20 14:25:22', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('431', '', '2016/10/20 14:26:39', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('432', '', '2016/10/20 14:26:55', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('433', '', '2016/10/20 14:28:15', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('434', '', '2016/10/20 14:29:37', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('435', '', '2016/10/20 14:29:48', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('436', '', '2016/10/20 14:30:40', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('437', '', '2016/10/20 14:31:58', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('438', '', '2016/10/20 14:32:40', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('439', '', '2016/10/20 14:33:09', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('440', '', '2016/10/20 14:33:52', '新建方案2', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('441', '', '2016/10/20 14:34:24', '新建方案3', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('442', '管理员', '2016/10/20 15:13:04', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('443', '管理员', '2016/10/20 15:16:30', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('444', '管理员', '2016/10/20 15:21:35', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('445', '', '2016/11/08 13:51:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('446', '', '2016/11/08 13:53:10', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('447', '', '2016/11/08 14:17:19', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('448', '', '2016/11/08 14:19:48', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('449', '', '2016/11/09 11:05:31', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('450', '', '2016/11/09 11:10:59', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('451', '', '2016/11/09 11:20:06', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('452', '', '2016/11/09 16:19:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('453', '', '2016/11/09 16:25:18', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('454', '', '2016/11/09 16:30:30', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('455', '', '2016/11/09 16:37:59', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('456', '', '2016/11/09 17:45:51', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('457', '', '2016/11/09 17:51:50', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('458', '', '2016/11/09 18:00:58', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('459', '', '2016/11/09 18:02:41', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('460', '', '2016/11/09 18:06:05', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('461', '', '2016/11/09 18:22:34', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('462', '', '2016/11/09 18:25:44', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('463', '', '2016/11/09 18:29:11', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('464', '', '2016/11/09 18:38:40', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('465', '', '2016/11/09 18:42:04', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('466', '', '2016/11/09 18:52:35', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('467', '', '2016/11/09 18:54:31', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('468', '', '2016/11/09 18:56:30', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('469', '', '2016/11/09 19:16:33', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('470', '', '2016/11/09 19:18:48', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('471', '', '2016/11/09 19:21:12', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('472', '', '2016/11/09 19:25:24', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('473', '', '2016/11/09 19:31:13', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('474', '', '2016/11/09 19:32:52', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('475', '', '2016/11/09 19:35:43', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('476', '', '2016/11/09 19:37:21', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('477', '', '2016/11/09 19:38:43', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('478', '', '2016/11/09 19:40:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('479', '', '2016/11/10 10:02:34', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('480', '', '2016/11/10 10:04:54', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('481', '', '2016/11/10 10:06:09', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('482', '', '2016/11/10 10:07:47', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('483', '', '2016/11/10 10:08:46', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('484', '', '2016/11/10 10:10:21', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('485', '', '2016/11/10 10:12:49', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('486', '', '2016/11/10 10:19:09', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('487', '', '2016/11/10 10:37:22', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('488', '', '2016/11/10 10:39:02', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('489', '', '2016/11/10 11:18:13', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('490', '', '2016/11/10 11:19:47', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('491', '', '2016/11/10 11:21:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('492', '', '2016/11/10 11:25:34', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('493', '', '2016/11/10 11:30:11', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('494', '', '2016/11/10 11:33:45', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('495', '', '2016/11/10 11:38:34', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('496', '', '2016/11/10 11:45:19', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('497', '', '2016/11/10 13:06:58', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('498', '', '2016/11/10 13:08:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('499', '', '2016/11/10 13:14:36', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('500', '', '2016/11/10 13:21:18', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('501', '', '2016/11/10 13:27:59', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('502', '', '2016/11/10 13:48:41', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('503', '', '2016/11/10 13:50:40', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('504', '', '2016/11/10 13:52:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('505', '', '2016/11/10 14:04:55', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('506', '', '2016/11/10 14:06:05', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('507', '', '2016/11/10 14:08:23', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('508', '', '2016/11/10 14:11:59', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('509', '', '2016/11/10 14:14:05', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('510', '', '2016/11/10 14:18:09', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('511', '', '2016/11/10 18:57:56', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('512', '', '2016/11/10 19:03:13', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('513', '', '2016/11/10 19:04:16', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('514', '', '2016/11/10 19:06:58', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('515', '', '2016/11/10 19:08:45', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('516', '', '2016/11/11 09:58:52', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('517', '', '2016/11/11 10:19:15', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('518', '', '2016/11/11 10:45:00', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('519', '', '2016/11/11 10:47:05', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('520', '', '2016/11/11 13:27:26', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('521', '', '2016/11/11 13:29:44', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('522', '', '2016/11/11 16:55:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('523', '', '2016/11/16 16:47:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('524', '', '2016/11/16 16:48:32', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('525', '', '2016/11/16 16:49:06', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('526', '', '2016/11/16 16:53:13', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('527', '', '2016/11/18 13:55:22', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('528', '', '2016/11/18 13:56:01', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('529', '', '2016/11/18 13:56:43', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('530', '', '2016/11/18 13:59:12', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('531', '', '2016/11/18 14:01:45', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('532', '', '2016/11/18 14:02:41', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('533', '', '2016/11/18 14:13:24', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('534', '', '2016/11/18 14:14:21', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('535', '', '2016/11/18 14:14:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('536', '', '2016/11/18 14:15:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('537', '', '2016/11/18 14:16:42', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('538', '', '2016/11/18 14:17:38', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('539', '', '2016/11/18 14:18:15', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('540', '', '2016/11/18 14:18:37', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('541', '', '2016/11/18 14:22:52', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('542', '', '2016/11/18 14:23:36', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('543', '', '2016/11/18 14:25:12', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('544', '', '2016/11/18 14:27:32', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('545', '', '2016/11/18 14:28:18', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('546', '', '2016/11/18 14:32:29', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('547', '', '2016/11/18 14:44:00', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('548', '', '2016/11/18 14:45:07', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('549', '', '2016/11/18 14:46:13', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('550', '', '2016/11/18 14:46:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('551', '', '2016/11/18 15:50:02', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('552', '', '2016/11/18 15:50:47', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('553', '', '2016/11/18 15:52:08', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('554', '', '2016/11/18 15:53:26', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('555', '', '2016/11/18 15:54:57', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('556', '', '2016/11/18 15:58:53', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('557', '', '2016/11/18 15:59:49', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('558', '', '2016/11/18 16:00:11', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('559', '', '2016/11/18 16:38:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('560', '', '2016/11/18 16:39:17', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('561', '', '2016/11/18 16:45:04', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('562', '', '2016/11/18 16:47:33', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('563', '', '2016/11/18 16:48:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('564', '', '2016/11/18 16:49:17', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('565', '', '2016/11/18 16:50:40', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('566', '', '2016/11/18 16:51:17', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('567', '', '2016/11/18 16:53:42', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('568', '', '2016/11/18 16:54:20', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('569', '', '2016/11/18 17:01:52', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('570', '', '2016/11/18 17:17:10', '新建方案1', '无');
INSERT INTO `cygj_video_patrol_log` VALUES ('571', '', '2016/11/18 17:20:06', '新建方案1', '无');

-- ----------------------------
-- Table structure for `cygj_video_patrol_plan`
-- ----------------------------
DROP TABLE IF EXISTS `cygj_video_patrol_plan`;
CREATE TABLE `cygj_video_patrol_plan` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `NAME` varchar(200) DEFAULT NULL,
  `DEVICEIDLIST` varchar(2000) DEFAULT NULL,
  `PLAYTIMELIST` varchar(2000) DEFAULT NULL,
  `ACCOUNTNAME` varchar(20) DEFAULT NULL,
  `CREATETIME` varchar(19) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cygj_video_patrol_plan
-- ----------------------------
INSERT INTO `cygj_video_patrol_plan` VALUES ('75', '新建方案1', '1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17', '5|5|5|5|5|5|5|5|5|5|5|5|5|5|5|5|5', 'admin', '2016-03-21 16:54:54');
INSERT INTO `cygj_video_patrol_plan` VALUES ('77', '新建方案2', '3|2|1', '5|5|5', 'admin', '2016-07-12 09:55:09');
INSERT INTO `cygj_video_patrol_plan` VALUES ('81', '新建方案3', '1', '5', 'admin', '2016-07-29 14:15:11');
