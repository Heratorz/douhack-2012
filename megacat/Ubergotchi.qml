import QtQuick 1.1
import "loader.js" as L

Rectangle {
    width: 196
    height: 196

    color: "#400040"

    Rectangle {
        width: parent.width * 0.125
        height: parent.height * 0.125
        color: "orange"
        x: width / 2
        y: height / 2
    }

    MegaCat {
        width: 256
        height: 256
        anchors.centerIn: parent
    }
}
