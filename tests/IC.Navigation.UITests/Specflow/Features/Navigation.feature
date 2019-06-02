@xunit:collection(UITests)
Feature: Navigation
	As a tester
	I want to the system to be able to navigate through the UI application 

Background: 
    Given The application under test has been started

@mytag
Scenario: Reuse steps declares in different Step classes
    # The dipose of each Step classes are called at the end of the scenario.
    # This example is explicitly forcing a path and should not be used as Gherkin example
    And The "menu view" has been opened
    Then The control "button to open the red view" should be displayed in the current view
    Given The "blue view" has been opened
    When I navigate to "menu view"
    Then The "menu view" should be opened
    And The control "button to open the red view" should be displayed in the current view
    And The control "fake name" should not be displayed in the current view

@mytag
Scenario: Navigate from the "menu view" to "blue view"
	Given The "menu view" has been opened
	When The "button to open the blue view" is pressed in current view
	Then The "blue view" should be opened

@mytag
Scenario: Navigate back to the "menu view" from "blue view"
	Given The "blue view" has been opened
	When The "button to go back to the previous view" is pressed in current view
	Then The "menu view" should be opened

@mytag
Scenario: Navigate to the "yellow view" from "blue view"
	Given The "blue view" has been opened
	When The "button to open the yellow view" is pressed in current view
	Then The "yellow view" should be opened

@mytag
Scenario Outline: The back button in "yellow view" should open the previous view
    Given The <view_before_yellow> has been opened
    When The <button_to_open_yellow_view> is pressed in current view
	Then The "yellow view" should be opened
    When The "button to go back to the previous view" is pressed in current view
    Then The <view_before_yellow> should be opened

   Examples: 
   | view_before_yellow | button_to_open_yellow_view           |
   | "menu view"        | "button to open the yellow view"     |
   | "blue view"        | "button to open the yellow view"     |
   | "red view"         | "button to open the yellow view"     |

@mytag
Scenario Outline: Navigate from a view to another and come back to the original view.
    Given The <original_view> has been opened
    When I navigate to <destination_view>
    Then The <destination_view> should be opened
    When I navigate to <original_view>
    Then The <original_view> should be opened

   Examples:
   | original_view | destination_view |
   | "menu view"   | "yellow view"    |
   | "blue view"   | "red view"       |
   | "red view"    | "yellow view"    |
   | "yellow view" | "menu view"      |


@mytag
Scenario Outline: Resolve navigation when an action can open more than one view.
    When I navigate to <view_opening_yellow_view>
    And I navigate to "yellow view"
    And I navigate to <a_different_than_view_opening_yellow_view>
    Then The <a_different_than_view_opening_yellow_view> should be opened

    Examples: 
    | view_opening_yellow_view | a_different_than_view_opening_yellow_view |
    | "blue view"              | "red view"                                |
    | "red view"               | "blue view"                               |
    | "menu view"              | "blue view"                               |
    | "menu view"              | "red view"                                |
    | "red view"               | "menu view"                               |
    | "blue view"              | "menu view"                               |
